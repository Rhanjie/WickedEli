using Characters.Interfaces;
using UnityEngine;
using Zenject;
using Terrain = Map.Terrain;

namespace Characters
{
    public abstract class LivingEntity : StaticEntity
    {
        protected IAttackBehaviour AttackBehaviour;
        protected IMovementBehaviour MovementBehaviour;
        
        private static readonly int VelocityHash = Animator.StringToHash("Velocity");

        public Transform LookAt
        {
            get => EntityReferences.lookAt;
            set
            {
                EntityReferences.lookAt = value;
                AttackBehaviour.SetTarget(value);
            }
        }

        protected override void Update()
        {
            base.Update();

            EntityReferences.animator.SetFloat(VelocityHash, MovementBehaviour.Velocity);
        }

        [Inject]
        public void Construct(IMovementBehaviour movementBehaviour, IAttackBehaviour attackBehaviour, Terrain terrain)
        {
            MovementBehaviour = movementBehaviour;
            AttackBehaviour = attackBehaviour;

            UpdateBehaviours();
        }

        private void UpdateBehaviours()
        {
            AttackBehaviour.SetTarget(EntityReferences.lookAt);
        }
    }
}