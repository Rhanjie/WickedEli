using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Map
{
    public class TerrainGenerator : MonoBehaviour
    {
        private TileData[,] _mapData;
        
        private TerrainGeneratorSettings _settings;
        
        [Inject] private Tilemap _tilemap;
        [Inject] private TilemapCollider2D _tilemapCollider;
        [Inject] private Transform _staticEntityPrefab;

        [Inject]
        private void Construct(TerrainGeneratorSettings settings)
        {
            _settings = settings;
        }

        [Button("Generate map")]
        private void GenerateMap()
        {
            if (!Application.isPlaying)
                InjectDependenciesInEditMode();

            //TODO: Fix size
            var noiseData = _settings.Noise.Generate(6);
            _mapData = new TileData[_settings.size, _settings.size];
            
            for (var y = 0; y < _mapData.GetLength(0); y++)
            {
                for (var x = 0; x < _mapData.GetLength(1); x++)
                {
                    var noiseValue = noiseData[y, x];
                    var index = Math.Abs((int) Math.Round(noiseValue));
                    var tileData = _settings.TryGetFromIndex(index);

                    if (!tileData.HasValue)
                        throw new Exception($"Not found tile with index: {index}");

                    var position = new Vector3Int(x, y, 0);
                    var worldPosition = _tilemap.CellToWorld(position);
                    var color = CalculateColor(tileData.Value, noiseValue);

                    GenerateTile(tileData.Value, position, color);
                    GenerateObject(tileData.Value, worldPosition);
                }
            }
            
            _tilemapCollider.ProcessTilemapChanges();
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
        }

        private void GenerateObject(TileData tileData, Vector3 position)
        {
            var objectSettings = tileData.GetRandomVariant(tileData.objects);
            if (objectSettings == null)
                return;

            var entity = Instantiate(_staticEntityPrefab, position, Quaternion.identity);
            var context = entity.GetComponent<GameObjectContext>();

            context.ScriptableObjectInstallers = new List<ScriptableObjectInstaller> { objectSettings };
        }

        public TileData[,] GetGeneratedTerrain()
        {
            if (_mapData == null)
                GenerateMap();

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
            _staticEntityPrefab = AssetDatabase.LoadAssetAtPath<Transform>(assetPath);
#endif
        }
    }
}