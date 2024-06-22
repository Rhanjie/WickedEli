using UnityEngine;

namespace Entities.Characters.Interfaces
{
    public interface IAttackBehaviour
    {
        public Transform LookAt { get; set; }

        void Attack();
    }
}