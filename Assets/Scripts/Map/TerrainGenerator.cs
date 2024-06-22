using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Zenject;

namespace Map
{
    public class TerrainGenerator : MonoBehaviour
    {
        [Inject] private Tilemap _tilemap;
        [Inject] private TilemapCollider2D _tilemapCollider;
        [Inject] private StaticEntity _staticEntityPrefab;

        private TileData[,] _mapData;
        private TerrainGeneratorSettings _settings;
        private DiContainer _diContainer;
        private float _progress;
        
        public static UnityAction<float> OnProgressUpdate;
        public static UnityAction OnProgressFinished;
        
        private const string GeneratedObjectsParentName = "Generated Objects";
        private const int MinNeighborsAmount = 3;
        private const float MaxProgress = 100;
        
        public float Progress
        {
            get => _progress;
            private set
            {
                _progress = value;
                
                if (_progress > MaxProgress)
                    _progress = MaxProgress;
                
                OnProgressUpdate?.Invoke(_progress / MaxProgress);
            }
        }

        [Inject]
        private void Construct(TerrainGeneratorSettings settings, DiContainer diContainer, StaticEntity staticEntityPrefab)
        {
            _settings = settings;
            _diContainer = diContainer;
            _staticEntityPrefab = staticEntityPrefab;
        }

        [Button("Generate map")]
        private async Task GenerateMap()
        {
            if (!Application.isPlaying)
                InjectDependenciesInEditMode();
            
            //TODO: Add UnityMainThreadDispatcher package
            
            var noiseData = await Task.Run(() => _settings.Noise.Generate((uint) _settings.size));
            _settings.size = noiseData.GetLength(0) - 1;
            
            Progress += 10;

            var indexData = await Task.Run(() => ConvertNoiseToIndexData(noiseData));
            
            Progress += 10;
            
            indexData = await Task.Run(() => SmoothTheGround(indexData));
            
            Progress += 10;

            StartCoroutine(GenerateMapData(noiseData, indexData));
        }
        
        private Task<int[,]> ConvertNoiseToIndexData(float[,] noiseData)
        {
            var indexData = new int[noiseData.GetLength(0), noiseData.GetLength(1)];
            
            for (var y = 0; y < noiseData.GetLength(0); y++)
            {
                for (var x = 0; x < noiseData.GetLength(1); x++)
                {
                    var noise = Math.Abs((int) Math.Round(noiseData[y, x]));
                    var tileIndex = _settings.TryGetTileIndexFromNoiseValue(noise);
                    
                    if (!tileIndex.HasValue)
                        throw new Exception($"Not found any tile for noise value: {noise}");

                    indexData[y, x] = tileIndex.Value;
                }
            }
            
            return Task.FromResult(indexData);
        }

        private Task<int[,]> SmoothTheGround(int[,] data)
        {
            for (var y = 1; y < data.GetLength(0) - 1; y++)
            {
                for (var x = 1; x < data.GetLength(1) - 1; x++)
                {
                    var index = data[y, x];
                    var neighbors = new Dictionary<int, int>();
                    for (var i = 0; i < _settings.tiles.Count; i++)
                        neighbors.Add(i, 0);

                    neighbors[data[y + 1, x]] += 1;
                    neighbors[data[y - 1, x]] += 1;
                    neighbors[data[y, x + 1]] += 1;
                    neighbors[data[y, x - 1]] += 1;
                    
                    neighbors[data[y + 1, x + 1]] += 1;
                    neighbors[data[y + 1, x - 1]] += 1;
                    neighbors[data[y - 1, x + 1]] += 1;
                    neighbors[data[y - 1, x - 1]] += 1;
                    
                    if (neighbors[index] >= MinNeighborsAmount)
                        continue;
                    
                    var mostPopularTile = neighbors.OrderByDescending(pair => pair.Value).First();
                    data[y, x] = mostPopularTile.Key;
                }
            }
            
            return Task.FromResult(data);
        }

        private IEnumerator GenerateMapData(float[,] noiseData, int[,] indexData)
        {
            var parent = GenerateObjectsParent();
            _mapData = new TileData[_settings.size, _settings.size];
            
            for (var y = 0; y < _mapData.GetLength(0); y++)
            {
                for (var x = 0; x < _mapData.GetLength(1); x++)
                {
                    var noise = noiseData[y, x];
                    var index = indexData[y, x];
                    
                    var tileData = _settings.tiles[index];
                    var color = CalculateColor(tileData, noise);
                    
                    var position = new Vector3Int(x, y, 0);
                    var worldPosition = _tilemap.GetCellCenterWorld(position);

                    GenerateTile(tileData, position, color);
                    GenerateObject(tileData, worldPosition, parent);
                }
                
                Progress += 1;
                yield return null;
            }
            
            Progress = MaxProgress;
            OnProgressFinished?.Invoke();
            
            _tilemapCollider.ProcessTilemapChanges();
        }
        
        private Transform GenerateObjectsParent()
        {
            var parent = transform.Find(GeneratedObjectsParentName);
            if (parent != null)
                DestroyImmediate(parent);
                
            parent = new GameObject(GeneratedObjectsParentName).transform;
            parent.parent = transform;

            return parent;
        }

        private Color CalculateColor(TileData tileData, float noiseValue)
        {
            var color = tileData.customColor;
            if (color == Color.clear)
                color = Color.white;

            var noiseColor = (Math.Abs(noiseValue) - tileData.indices.x) * tileData.heightColorAddition;

            color.r = (byte)Mathf.Clamp(color.r - noiseColor, 0, 255);
            color.g = (byte)Mathf.Clamp(color.g - noiseColor, 0, 255);
            color.b = (byte)Mathf.Clamp(color.b - noiseColor, 0, 255);
            color.a = 255;

            return color;
        }

        private void GenerateTile(TileData tileData, Vector3Int position, Color color)
        {
            var tile = tileData.GetRandomVariant(tileData.variants);
            var colliderType = tileData.walkable
                ? Tile.ColliderType.None
                : Tile.ColliderType.Grid;

            _tilemap.SetTile(position, tile);
            _tilemap.SetTileFlags(position, TileFlags.None);
            _tilemap.SetColor(position, color);
            _tilemap.SetColliderType(position, colliderType);
            
            _mapData[position.y, position.x] = tileData;
            _mapData[position.y, position.x].customColor = color;
        }

        private void GenerateObject(TileData tileData, Vector3 position, Transform parent)
        {
            var objectSettings = tileData.GetRandomVariant(tileData.objects);
            if (objectSettings == null)
                return;
            
            var context = _staticEntityPrefab.GetComponent<GameObjectContext>();
            context.ScriptableObjectInstallers = new List<ScriptableObjectInstaller> { objectSettings };

            var entity = _diContainer.InstantiatePrefab(
                _staticEntityPrefab, position, Quaternion.identity, parent
            );
        }

        public async Task<TileData[,]> GetGeneratedTerrain()
        {
            if (_mapData == null)
                await GenerateMap();

            return _mapData;
        }

        private void InjectDependenciesInEditMode()
        {
#if UNITY_EDITOR
            _settings = GetComponent<TerrainReferencesInstaller>().settings;
            _tilemap = GetComponentInChildren<Tilemap>();
            _tilemapCollider = GetComponentInChildren<TilemapCollider2D>();

            const string staticEntityPrefabGuid = "5b501fee22682d0469142cd24662015d";
            var assetPath = AssetDatabase.GUIDToAssetPath(staticEntityPrefabGuid);
            //_staticEntityPrefab = AssetDatabase.LoadAssetAtPath<Transform>(assetPath);
#endif
        }
    }
}