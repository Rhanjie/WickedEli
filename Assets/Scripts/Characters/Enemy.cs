using UnityEngine;

namespace Characters
{
    public class Enemy : Character
    {
        protected override void Update()
        {
            base.Update();
        
            FindTarget();
        
            if (LookAt != null)
                FollowTarget();
        
            else movement.Stop();
        }

        private void FollowTarget()
        {
            if (IsTargetInRange())
                attack.Attack();
        
            else movement.Move(GetDirectionToTarget());
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

            return distance <= settings.range;
        }

        private void FindTarget()
        {
            var position = body.transform.position;
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
            var position = body.transform.position;
            var size = new Vector2(20, 13);
        
            Gizmos.DrawWireCube(position, size);
        }
    }
}