using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Map
{
    [Serializable]
    public struct TileData : IComparable
    {
        [Title("Parameters")] [MinMaxSlider(0, 1000, true)]
        public Vector2 indices;

        public Color32 color;
        public List<TileBase> variants;

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

            var index = Random.Range(0, variants.Count);

            return variants[index];
        }

        public int CompareTo(TileData compared)
        {
            return indices.x.CompareTo(compared.indices.y);
        }
    }
}