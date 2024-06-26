﻿using System.Collections.Generic;
using Entities.Characters.Players;
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
        [OdinSerialize] public INoise Noise;
        
        [Space]
        
        public List<TileData> tiles;

        private void OnValidate()
        {
            //tiles.Sort();
        }

        public int? TryGetTileIndexFromNoiseValue(int index)
        {
            for (var i = 0; i < tiles.Count; i++)
                if (index >= tiles[i].indices.x && index <= tiles[i].indices.y)
                    return i;

            return null;
        }
    }
}