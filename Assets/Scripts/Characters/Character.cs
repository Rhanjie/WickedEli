using UnityEngine;
using Characters.Players;

namespace Characters
{
    public class Character : LivingEntity
    {
        protected override void Update()
        {
            base.Update();
        
            FindTarget();
        
            if (LookAt != null)
                FollowTarget();
        
            else Movement.Stop();
        }

        private void FollowTarget()
        {
            if (IsTargetInRange())
                MeleeAttack.Attack();
        
            else Movement.Move(GetDirectionToTarget());
        }

        private Vector2 GetDirectionToTarget()
        {
            var position = transform.position;
            var targetPosition = LookAt.transform.position;
            var direction = new Vector2(targetPosition.x - position.x, targetPosition.y - position.y);

            return direction;
        }

        private bool IsTargetInRange()
        {
            var position = transform.position;
            var targetPosition = LookAt.transform.position;

            var distance = new Vector2(targetPosition.x - position.x, targetPosition.y - position.y).magnitude;

            return distance <= EntitySettings.range;
        }

        private void FindTarget()
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
        
            LookAt = player.transform;
        }

        private void ResetTarget()
        {
            LookAt = null;
        }

        public override void Destroy()
        {
            //TODO: Effect
        
            gameObject.SetActive(false);
        }
    
        private void OnDrawGizmos()
        {
            var position = EntityReferences.body.transform.position;
            var size = new Vector2(20, 13);
        
            Gizmos.DrawWireCube(position, size);
        }
    }
}