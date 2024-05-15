using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Random = UnityEngine.Random;

namespace Terrain
{
    public class TerrainGenerator : MonoBehaviour
    {
        [Serializable]
        public struct Settings
        {
            public int size;
            public List<TileData> tiles;

            private void OnValidate()
            {
                tiles.Sort();
            }

            public TileData? TryGetFromIndex(int index)
            {
                for (var i = 0; i < tiles.Count; i++)
                {
                    if (index >= tiles[i].indices.x && index <= tiles[i].indices.y)
                        return tiles[i];
                }

                return null;
            }
        }
    
        [Serializable]
        public struct References
        {
            public Tilemap tilemap;
        }
    
        private Settings _settings;
        private References _references;
    
        //TODO: Add factory
        private INoise _noise;
        private int[,] _mapData;

        [Inject]
        public void Construct(Settings settings, References references)
        {
            _settings = settings;
            _references = references;
            
            GenerateMap();
        }

        [Button("Generate map")]
        private void GenerateMap()
        {
            //TODO: HOW TO INJECT DEPENDENCIES IN EDIT MODE
            //SceneContext.Create();
            //GetComponent<GameObjectContext>().Run();
            
            _references.tilemap.ClearAllTiles();

            _noise = new DiamondSquareNoise();
            var noiseData = _noise.Generate(6, Random.Range(0, 20000), 0.5f);
        
            _mapData = new int[_settings.size, _settings.size];
            for (var y = 0; y < _mapData.GetLength(0); y++)
            {
                for (var x = 0; x < _mapData.GetLength(1); x++)
                {
                    var noiseValue = noiseData[y, x];
                
                    var index = (int) Math.Round(noiseValue);
                    var tileData = _settings.TryGetFromIndex(index);
                
                    if (tileData == null)
                        throw new Exception($"Not found tile with index: {index}");

                    var position = new Vector3Int(x, y, 0);
                    var noiseColor = (byte)(255f - noiseValue * 5f);
                    var color = tileData.Value.color;

                    color.r += noiseColor;
                    color.g += noiseColor;
                    color.b += noiseColor;
                    color.a = 255;
                
                    _references.tilemap.SetTile(position, tileData.Value.GetRandomVariant());
                    _references.tilemap.SetTileFlags(position, TileFlags.None);
                    _references.tilemap.SetColor(position, color);
                }
            }

            CenterTerrain();
        }

        private void CenterTerrain()
        {
            transform.position = new Vector3(0, -_mapData.GetLength(0), 0f);
        }
    }
}
