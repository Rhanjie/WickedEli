using Entities.Characters.Players;
using UnityEngine;

namespace Entities.Characters
{
    public class Character : LivingEntity
    {
        protected override void Update()
        {
            base.Update();
            
            TryToFindTarget();

            if (AttackBehaviour.LookAt == null)
                MovementBehaviour.Stop();

            else FollowTarget();
        }
        
        public override void Destroy()
        {
            //TODO: Effect
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

            return distance <= EntitySettings.range;
        }

        private void TryToFindTarget()
        {
            var position = EntityReferences.body.transform.position;
            var size = new Vector2(20, 13);
            var layerMask = LayerMask.GetMask("Player");

            var result = Physics2D.OverlapBox(position, size, 0, layerMask);

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
    }
}