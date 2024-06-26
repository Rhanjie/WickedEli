﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Map
{
    [Serializable]
    public struct TileData : IComparable
    {
        [Space] [Space] [Space]
        [Title("General")]
        public string name;
        [MinMaxSlider(0, 1000, true)]
        public Vector2Int indices;
        
        [Title("Appearance")]
        public Color32 customColor;
        [Range(-20, 20)]
        public float heightColorAddition;
        
        [Title("Tiles section")]
        
        [SerializeField] [TableList] [LabelText("Tile variants")]
        public List<Generable<TileBase>> variants;
        
        [Title("Objects section")]
        [SerializeField] [TableList]
        public List<Generable<Object>> objects;

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

        public int CompareTo(TileData compared)
        {
            return indices.x.CompareTo(compared.indices.y);
        }
    }
}