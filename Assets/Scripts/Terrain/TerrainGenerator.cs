using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Random = UnityEngine.Random;

namespace Terrain
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] [LabelText("Reference needed only in edit mode")]
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
            var tilemap = GetComponentInChildren<Tilemap>();
            var noiseData = settings.Noise.Generate(6, Random.Range(0, 20000), 0.5f);
        
            _mapData = new TileData[settings.size, settings.size];
            for (var y = 0; y < _mapData.GetLength(0); y++)
            {
                for (var x = 0; x < _mapData.GetLength(1); x++)
                {
                    var noiseValue = noiseData[y, x];
                
                    var index = (int) Math.Round(noiseValue);
                    var tileData = settings.TryGetFromIndex(index);
                
                    if (tileData == null)
                        throw new Exception($"Not found tile with index: {index}");

                    var position = new Vector3Int(x, y, 0);
                    var noiseColor = (byte)(255f - noiseValue * 5f);
                    var color = tileData.Value.color;

                    color.r += noiseColor;
                    color.g += noiseColor;
                    color.b += noiseColor;
                    color.a = 255;
                
                    tilemap.SetTile(position, tileData.Value.GetRandomVariant());
                    tilemap.SetTileFlags(position, TileFlags.None);
                    tilemap.SetColor(position, color);
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
