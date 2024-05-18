using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Terrain
{
    [RequireComponent(typeof(TerrainGenerator))]
    public class Terrain : MonoBehaviour
    {
        private TileData[,] _mapData;
        private TerrainGenerator _terrainGenerator;
        
        public Tilemap Tilemap { get; private set; }

        [Inject]
        private void Construct(Tilemap tilemap, TerrainGenerator terrainGenerator)
        {
            Tilemap = tilemap;
            _terrainGenerator = terrainGenerator;
            _mapData = _terrainGenerator.GetGeneratedTerrain();
            
            CenterTerrain();
        }

        public void SetTileAtCell(int x, int y, TileData data)
        {
            if (IsOutsideMap(x, y))
                throw new ArgumentOutOfRangeException();
            
            _mapData[y, x] = data;
        }

        public TileData GetTile(int x, int y)
        {
            if (IsOutsideMap(x, y))
                throw new ArgumentOutOfRangeException();
            
            return _mapData[y, x];
        }

        private bool IsOutsideMap(int x, int y)
        {
            return y < 0 || x < 0 || y >= _mapData.GetLength(0) || x >= _mapData.GetLength(1);
        }
        
        private void CenterTerrain()
        {
            transform.position = new Vector3(0, -_mapData.GetLength(0), 0f);
        }
    }
}