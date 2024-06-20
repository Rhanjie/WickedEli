using UnityEngine;

namespace Characters.Interfaces
{
    public interface IAttackBehaviour
    {
        public Transform LookAt { get; set; }

        void Attack();
    }
}