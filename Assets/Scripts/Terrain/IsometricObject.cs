using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

//https://www.evozon.com/two-unity-tricks-isometric-games/
namespace Terrain
{
    public class IsometricObject : MonoBehaviour
    {
        [Serializable]
        public class References
        {
            public Transform attachedTo;
            public int offset;
        }
    
        private const int IsometricRangePerYUnit = 100;

        private References _references;
        private List<Renderer> _renderers;

        [Inject]
        private void Construct(References references, List<Renderer> renderers)
        {
            _references = references;
            _renderers = renderers;
            
            if (_references.attachedTo == null)
                _references.attachedTo = transform;
        }

        protected virtual void Update()
        {
            foreach (var rendererComponent in _renderers)
            { 
                rendererComponent.sortingOrder = -(int)(_references.attachedTo.position.y * IsometricRangePerYUnit) + _references.offset;
            }
        }
    }
}
