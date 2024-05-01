using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainSettings", menuName = "Settings/Terrain")]
public class TerrainGeneratorSettings : SerializedScriptableObject
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