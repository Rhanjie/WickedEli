using Characters.Behaviours;
using Characters.Interfaces;
using UnityEngine;
using Zenject;

namespace Characters
{
    public abstract class LivingEntity : StaticEntity
    {
        protected IMovementBehaviour MovementBehaviour;
        protected IAttackBehaviour AttackBehaviour;

        public Transform LookAt
        {
            get => EntityReferences.lookAt;
            set
            {
                EntityReferences.lookAt = value;
                AttackBehaviour.SetTarget(value);
            }
        }
        
        private static readonly int VelocityHash = Animator.StringToHash("Velocity");

        [Inject]
        public void Construct(IMovementBehaviour movementBehaviour, IAttackBehaviour attackBehaviour)
        {
            MovementBehaviour = movementBehaviour;
            AttackBehaviour = attackBehaviour;

            UpdateBehaviours();
        }

        private void UpdateBehaviours()
        {
            AttackBehaviour.SetTarget(EntityReferences.lookAt);
        }
    
        protected override void Update()
        {
            base.Update();
        
            EntityReferences.animator.SetFloat(VelocityHash, MovementBehaviour.Velocity);
        }
    }
}