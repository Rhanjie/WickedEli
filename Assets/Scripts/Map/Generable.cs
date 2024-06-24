using System;
using Sirenix.OdinInspector;

namespace Map
{
    [Serializable]
    public class Generable<T>
    {
        [TableColumnWidth(100, Resizable = false)]
        
        public int chance;
        [AssetsOnly]
        public T generableObject;
    }
}