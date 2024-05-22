using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Map
{
    public class TerrainGenerator : MonoBehaviour
    {
        private TileData[,] _mapData;
        
        private TerrainGeneratorSettings _settings;
        private Tilemap _tilemap;
        private TilemapCollider2D _tilemapCollider;

        [Inject]
        private void Construct(TerrainGeneratorSettings settings, Tilemap tilemap, TilemapCollider2D tilemapCollider)
        {
            _settings = settings;
            _tilemap = tilemap;
            _tilemapCollider = tilemapCollider;
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

                    var index = Math.Abs((int)Math.Round(noiseValue));
                    var tileData = _settings.TryGetFromIndex(index);

                    if (tileData == null)
                        throw new Exception($"Not found tile with index: {index}");

                    var position = new Vector3Int(x, y, 0);
                    var color = tileData.Value.color;
                    if (color == Color.clear)
                        color = Color.white;

                    var noiseColor = noiseValue * tileData.Value.heightColorAddition;

                    color.r = (byte)Mathf.Clamp(color.r - noiseColor, 0, 255);
                    color.g = (byte)Mathf.Clamp(color.g - noiseColor, 0, 255);
                    color.b = (byte)Mathf.Clamp(color.b - noiseColor, 0, 255);
                    color.a = 255;

                    var tile = tileData.Value.GetRandomVariant();
                    var colliderType = tileData.Value.walkable
                        ? Tile.ColliderType.None
                        : Tile.ColliderType.Sprite;

                    _tilemap.SetTile(position, tile);
                    _tilemap.SetTileFlags(position, TileFlags.None);
                    _tilemap.SetColor(position, color);
                    _tilemap.SetColliderType(position, colliderType);

                    _mapData[y, x] = tileData.Value;
                    _mapData[y, x].color = color;
                }
            }

            _tilemapCollider.ProcessTilemapChanges();
        }

        public TileData[,] GetGeneratedTerrain()
        {
            if (_mapData == null)
                GenerateMap();

            return _mapData;
        }

        private void InjectDependenciesInEditMode()
        {
            _settings = GetComponent<TerrainReferencesInstaller>().settings;
            _tilemap = GetComponentInChildren<Tilemap>();
            _tilemapCollider = GetComponentInChildren<TilemapCollider2D>();
        }
    }
}