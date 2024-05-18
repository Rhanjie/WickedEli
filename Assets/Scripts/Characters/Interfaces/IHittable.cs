using UnityEngine;

namespace Characters.Interfaces
{
    interface IHittable
    {
        public Transform Handler { get; }

        public void Hit(int damage);
    }
}