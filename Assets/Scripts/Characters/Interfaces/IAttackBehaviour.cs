using UnityEngine;

namespace Characters.Interfaces
{
    public interface IAttackBehaviour
    {
        void Attack();

        void SetTarget(Transform target);
    }
}