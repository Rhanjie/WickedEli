using System;
using Sirenix.OdinInspector;
using Terrain.Installers;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Terrain
{
    public class TerrainGenerator : MonoBehaviour
    {
        private TerrainGeneratorSettings settings;
        private TileData[,] _mapData;

        [Inject]
        private void Construct(TerrainGeneratorSettings injectedSettings)
        {
            settings = injectedSettings;
        }

        [Button("Generate map")]
        private void GenerateMap()
        {
#if UNITY_EDITOR
            settings = GetComponent<TerrainReferencesInstaller>().settings;
#endif
            
            var tilemap = GetComponentInChildren<Tilemap>();
            var noiseData = settings.Noise.Generate(6);
        
            _mapData = new TileData[settings.size, settings.size];
            for (var y = 0; y < _mapData.GetLength(0); y++)
            {
                for (var x = 0; x < _mapData.GetLength(1); x++)
                {
                    var noiseValue = noiseData[y, x];
                
                    var index = Math.Abs((int) Math.Round(noiseValue));
                    var tileData = settings.TryGetFromIndex(index);
                
                    if (tileData == null)
                        throw new Exception($"Not found tile with index: {index}");

                    var position = new Vector3Int(x, y, 0);
                    var color = tileData.Value.color;
                    if (color == Color.clear)
                        color = Color.white;
                    
                    var noiseColor = noiseValue * 5f;

                    color.r = (byte) Mathf.Clamp(color.r - noiseColor, 0, 255);
                    color.g = (byte) Mathf.Clamp(color.g - noiseColor, 0, 255);
                    color.b = (byte) Mathf.Clamp(color.b - noiseColor, 0, 255);
                    color.a = 255;
                
                    tilemap.SetTile(position, tileData.Value.GetRandomVariant());
                    tilemap.SetTileFlags(position, TileFlags.None);
                    tilemap.SetColor(position, color);

                    _mapData[y, x] = tileData.Value;
                    _mapData[y, x].color = color;
                }
            }
        }

        public TileData[,] GetGeneratedTerrain()
        {
            if (_mapData == null)
                GenerateMap();
            
            return _mapData;
        }
    }
}
