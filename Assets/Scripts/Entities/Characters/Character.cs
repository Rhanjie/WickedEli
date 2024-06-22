using System;
using Entities.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Entities.Characters
{
    public class Character : LivingEntity
    {
        protected override void Update()
        {
            base.Update();

            if (IsDead)
                return;
            
            TryToFindTarget();

            if (AttackBehaviour.LookAt == null)
                MovementBehaviour.Stop();

            else FollowTarget();
        }
        
        public override void Destroy()
        {
            base.Destroy();
            
            MovementBehaviour.Stop();
        }

        private void FollowTarget()
        {
            if (IsTargetInRange())
                AttackBehaviour.Attack();

            else MovementBehaviour.Move(GetDirectionToTarget());
        }

        private Vector2 GetDirectionToTarget()
        {
            var position = transform.position;
            var targetPosition = AttackBehaviour.LookAt.transform.position;
            var direction = new Vector2(targetPosition.x - position.x, targetPosition.y - position.y);

            return direction;
        }

        private bool IsTargetInRange()
        {
            var position = transform.position;
            var targetPosition = AttackBehaviour.LookAt.transform.position;

            var distance = new Vector2(targetPosition.x - position.x, targetPosition.y - position.y).magnitude;

            return distance <= 2f;
        }

        private void TryToFindTarget()
        {
            //TODO: Refactor this method
            
            var position = EntityReferences.body.transform.position;
            var layerMask = LayerMask.GetMask("Player");

            var result = Physics2D.OverlapCircle(position, EntitySettings.visionRange, layerMask);

            var player = result != null ? result.GetComponent<Player>() : null;
            if (player == null)
            {
                ResetTarget();

                return;
            }

            AttackBehaviour.LookAt = player.transform;
        }

        private void ResetTarget()
        {
            AttackBehaviour.LookAt = null;
        }
        
        [Serializable]
        public class Settings
        {
            public enum Attitude { Hostile, Neutral, Friendly };

            [Title("General")]
            public Attitude attitude = Attitude.Neutral;
        }
    }
}