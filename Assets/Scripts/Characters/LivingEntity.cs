using Characters.Behaviours;
using UnityEngine;
using Zenject;

namespace Characters
{
    public abstract class LivingEntity : StaticEntity
    {
        protected MovementBehaviour Movement;
        protected MeleeAttackBehaviour MeleeAttack;

        private static readonly int Velocity = Animator.StringToHash("Velocity");
        
        public Transform LookAt
        {
            get => EntityReferences.lookAt;
            set
            {
                EntityReferences.lookAt = value;
            
                Movement.SetTarget(value);
                MeleeAttack.SetTarget(value);
            }
        }

        [Inject]
        public void Construct(MovementBehaviour movement, MeleeAttackBehaviour meleeAttack)
        {
            Movement = movement;
            MeleeAttack = meleeAttack;

            UpdateBehaviours();
        }

        private void UpdateBehaviours()
        {
            Movement.SetTarget(EntityReferences.lookAt);
            MeleeAttack.SetTarget(EntityReferences.lookAt);
        }
    
        protected override void Update()
        {
            base.Update();
        
            EntityReferences.animator.SetFloat(Velocity, Movement.Velocity);
        }
    }
}