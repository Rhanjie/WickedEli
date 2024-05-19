using UnityEngine;

namespace Characters.Interfaces
{
    public interface IHittable
    {
        public Transform Handler { get; }

        public void Hit(int damage);
    }
}