using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private TerrainGeneratorSettings settings;
    
    private INoise noise;
    
    private Tilemap _tilemap;
    private int[,] _mapData;
    
    private void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
    }

    private void OnValidate()
    {
        if (_tilemap == null)
            _tilemap = GetComponentInChildren<Tilemap>();
    }

    [Button("Generate map")]
    private void GenerateMap()
    {
        _tilemap.ClearAllTiles();

        noise = new DiamondSquareNoise();
        var noiseData = noise.Generate(6, Random.Range(0, 20000));
        
        var mapPrettyPrint = "";
        _mapData = new int[settings.size, settings.size];
        for (var y = 0; y < _mapData.GetLength(0); y++)
        {
            for (var x = 0; x < _mapData.GetLength(1); x++)
            {
                var noiseValue = noiseData[y, x];
                mapPrettyPrint += $"{noiseValue}, ";

                //TODO: Add range
                var id = Math.Abs(noiseValue);
                if (id > 10)
                    id = 10;
                
                var position = new Vector3Int(x, y, 0);

                if (!settings.tiles.ContainsKey(id))
                    throw new Exception($"Not found tile with ID: {id}");

                _tilemap.SetTile(position, settings.tiles[id]);
            }
            
            mapPrettyPrint += "\n";
        }

        Debug.LogError(mapPrettyPrint);

        CenterTerrain();
    }

    private void CenterTerrain()
    {
        transform.position = new Vector3(0, -_mapData.GetLength(0), 0f);
    }

    private void Update()
    {
        
    }
}
