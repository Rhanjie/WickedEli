using System.Collections.Generic;
using Map.Noises;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "TerrainGeneratorSettings", menuName = "Settings/TerrainGenerator")]
    public class TerrainGeneratorSettings : SerializedScriptableObject
    {
        public int size;
        public List<TileData> tiles;

        [OdinSerialize] public INoise Noise;

        private void OnValidate()
        {
            tiles.Sort();
        }

        public TileData? TryGetFromIndex(int index)
        {
            for (var i = 0; i < tiles.Count; i++)
                if (index >= tiles[i].indices.x && index <= tiles[i].indices.y)
                    return tiles[i];

            return null;
        }
    }
}