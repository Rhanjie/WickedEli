using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Map
{
    [Serializable]
    public struct TileVariant
    {
        [TableColumnWidth(100, Resizable = false)]
        public int chance;
        public TileBase tile;
    }
    
    [Serializable]
    public struct TileData : IComparable
    {
        [Title("Parameters")] [MinMaxSlider(0, 1000, true)]
        public Vector2 indices;

        public Color32 color;
        [ShowInInspector] [TableList]
        public List<TileVariant> variants;
        
        [Range(-10, 10)]
        public float heightColorAddition;

        [Title("Settings")]
        public bool walkable;
        
        public bool hurtable;
        [ShowIf("hurtable")]
        public int damage;

        [Range(0, 100)]
        public float friction;

        public int CompareTo(object obj)
        {
            if (obj is not TileData compared)
                return 1;

            return indices.x.CompareTo(compared.indices.y);
        }

        public TileBase GetRandomVariant()
        {
            if (variants == null || variants.Count == 0)
                return null;
            
            if (variants.Count == 1)
                return variants[0].tile;
            
            var poolSize = 0;
            for (var i = 0; i < variants.Count; i++)
            {
                poolSize += variants[i].chance;
            }

            var randomNumber = Random.Range(0, poolSize) + 1;
            
            var accumulatedProbability = 0;
            for (var i = 0; i < variants.Count; i++)
            {
                accumulatedProbability += variants[i].chance;
                if (randomNumber <= accumulatedProbability)
                    return variants[i].tile;
            }

            return null;
        }

        public int CompareTo(TileData compared)
        {
            return indices.x.CompareTo(compared.indices.y);
        }
    }
}