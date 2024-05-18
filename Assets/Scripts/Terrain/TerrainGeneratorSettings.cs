using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Terrain.Noises;
using UnityEngine;

namespace Terrain
{
    [CreateAssetMenu(fileName = "TerrainGeneratorSettings", menuName = "Settings/TerrainGenerator")]
    public class TerrainGeneratorSettings : SerializedScriptableObject
    {
        [OdinSerialize] 
        public INoise Noise;
        
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
}