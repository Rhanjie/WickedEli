using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Map
{
    [Serializable]
    public class Generable<T>
    {
        [TableColumnWidth(100, Resizable = false)]
        [field: SerializeField] public int Chance { get; set; }
        [field: SerializeField] public T Object { get; set; }
    }
    
    [Serializable]
    public struct TileData : IComparable
    {
        [Title("General")]
        public string name;
        [MinMaxSlider(0, 1000, true)]
        public Vector2Int indices;
        
        [Title("Appearance")]
        public Color32 customColor;
        [Range(-20, 20)]
        public float heightColorAddition;
        
        [Title("Tiles section")]
        
        [SerializeField] [TableList]
        public List<Generable<TileBase>> variants;
        
        [SerializeField] [TableList]
        public List<Generable<StaticEntitySettingsInstaller>> objects;

        [Title("Settings")]
        public bool walkable;
        public bool hurtable;
        
        [Space]
        
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

        public T GetRandomVariant<T>(List<Generable<T>> generables)
        {
            if (generables == null || generables.Count == 0)
                return default;
            
            if (generables.Count == 1)
                return generables[0].Object;
            
            var poolSize = generables.Sum(t => t.Chance);
            var randomNumber = Random.Range(0, poolSize) + 1;
            
            var accumulatedProbability = 0;
            for (var i = 0; i < generables.Count; i++)
            {
                accumulatedProbability += generables[i].Chance;
                if (randomNumber <= accumulatedProbability)
                    return generables[i].Object;
            }

            return default;
        }

        public int CompareTo(TileData compared)
        {
            return indices.x.CompareTo(compared.indices.y);
        }
    }
}