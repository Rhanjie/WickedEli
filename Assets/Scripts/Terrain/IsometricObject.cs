using System;
using UnityEngine;
using Zenject;

//https://www.evozon.com/two-unity-tricks-isometric-games/
namespace Terrain
{
    public class IsometricObject : MonoBehaviour
    {
        [Serializable]
        public abstract class References
        {
            public Renderer[] renderers;
            public Transform attachedTo;
            public int offset;
        }
    
        private const int IsometricRangePerYUnit = 100;

        private References _references;

        [Inject]
        private void Construct(References references)
        {
            _references = references;
            
            if (_references.attachedTo == null)
                _references.attachedTo = transform;
        }

        protected virtual void Update()
        {
            foreach (var rendererComponent in _references.renderers)
            { 
                rendererComponent.sortingOrder = -(int)(_references.attachedTo.position.y * IsometricRangePerYUnit) + _references.offset;
            }
        }
    }
}
