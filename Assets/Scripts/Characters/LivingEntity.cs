using Characters.Behaviours;
using UnityEngine;
using Zenject;

namespace Characters
{
    public abstract class LivingEntity : StaticEntity
    {
        protected MovementBehaviour Movement;
        protected AttackBehaviour Attack;

        private static readonly int Velocity = Animator.StringToHash("Velocity");
        
        public Transform LookAt
        {
            get => EntityReferences.lookAt;
            set
            {
                EntityReferences.lookAt = value;
            
                Movement.SetTarget(value);
                Attack.SetTarget(value);
            }
        }

        [Inject]
        public void Construct(MovementBehaviour movement, AttackBehaviour attack)
        {
            Movement = movement;
            Attack = attack;

            UpdateBehaviours();
        }

        private void UpdateBehaviours()
        {
            Movement.SetTarget(EntityReferences.lookAt);
            Attack.SetTarget(EntityReferences.lookAt);
        }
    
        protected override void Update()
        {
            base.Update();
        
            EntityReferences.animator.SetFloat(Velocity, Movement.Velocity);
        }
    }
}