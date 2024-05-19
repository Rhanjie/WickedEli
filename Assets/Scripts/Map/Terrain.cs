using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Map
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

        public TileData GetTileAtCell(int x, int y)
        {
            if (IsOutsideMap(x, y))
                return default;

            return _mapData[y, x];
        }

        public TileData GetTileAtPosition(Vector3 position)
        {
            var cell = Tilemap.WorldToCell(position);

            return GetTileAtCell(cell.x, cell.y);
        }

        public void SetTileAtCell(int x, int y, TileData data)
        {
            if (IsOutsideMap(x, y))
                throw new ArgumentOutOfRangeException();

            _mapData[y, x] = data;
        }

        public void SetTileAtPosition(Vector3 position, TileData data)
        {
            var cell = Tilemap.WorldToCell(position);

            SetTileAtCell(cell.x, cell.y, data);
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