using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TerrainSettings", menuName = "Settings/Terrain")]
public class TerrainGeneratorSettings : SerializedScriptableObject
{
    public int width;
    public int height;
    
    public Dictionary<int, TileBase> tiles;
}