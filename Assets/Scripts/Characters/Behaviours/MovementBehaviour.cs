using System;
using Characters.Interfaces;
using Characters.Settings;
using UnityEngine;
using Zenject;

namespace Characters.Behaviours
{
    public class MovementBehaviour : IMoveable
    {
        [Serializable]
        public class Settings
        {
            public float speed;
            public float acceleration;
            public float friction;
        }
        
        public Vector2 Position { get; private set; }
        public float Velocity { get; private set; }
        public bool IsFacingRight { get; private set; }

        public class References
        {
            public Transform body;
            public Rigidbody2D physics;
        }
        
        private References _references;
        private Settings _settings;
        private Transform _lookAt;
    
        private float _horizontalMove;
        private float _verticalMove;

        [Inject]
        public void Construct(References references, Settings settings)
        {
            _references = references;
            _settings = settings;
        }

        private void FixedUpdate()
        {
            if (_lookAt != null)
                CalculateTargetDirection();
        
            var movement = CalculateMovement();
            ApplyForce(movement, ForceMode2D.Force);

            var friction = CalculateFriction();
            ApplyForce(friction, ForceMode2D.Impulse);
        
            Velocity = _references.physics.velocity.magnitude;
        }
    
        public void Move(Vector2 delta)
        {
            delta = delta.normalized;
        
            _horizontalMove = delta.x;
            _verticalMove = delta.y;
        }

        public void Stop()
        {
            _references.physics.velocity = Vector3.zero;
            _references.physics.angularVelocity = 0;

            _horizontalMove = 0;
            _verticalMove = 0;
        }
    
        private Vector2 CalculateMovement()
        {
            var targetSpeed = new Vector2(_horizontalMove, _verticalMove).normalized * _settings.speed;
            var speedDifference = targetSpeed - _references.physics.velocity;
        
            var movement = speedDifference * _settings.acceleration;
            return movement;
        }

        private Vector2 CalculateFriction()
        {
            if (Mathf.Abs(_horizontalMove) >= 0.01f || Mathf.Abs(_verticalMove) >= 0.01f)
                return Vector2.zero;

            var physics = _references.physics;
            
            var frictionX = Mathf.Min(Mathf.Abs(physics.velocity.x), Mathf.Abs(_settings.friction));
            frictionX *= -Mathf.Sign(physics.velocity.x);
        
            var frictionY = Mathf.Min(Mathf.Abs(physics.velocity.y), Mathf.Abs(_settings.friction));
            frictionY *= -Mathf.Sign(physics.velocity.y);
        
            return new Vector2(frictionX, frictionY);
        }
    
        public void ApplyForce(Vector2 force, ForceMode2D mode)
        {
            if (force == Vector2.zero)
                return;
        
            _references.physics.AddForce(force, mode);
        }
    
        public void SetTarget(Transform target)
        {
            _lookAt = target;
        }

        private void CalculateTargetDirection()
        {
            var direction = GetDirectionToMouse();
            if (ShouldBeFlipped(direction.x))
                Flip();
        }

        private Vector2 GetDirectionToMouse()
        {
            var targetPosition = _lookAt.position;
            var handPosition = transform.position;
            var direction = new Vector2(
                handPosition.x - targetPosition.x, 
                handPosition.y - targetPosition.y
            );

            return direction;
        }
    
        private bool ShouldBeFlipped(float horizontalDirection)
        {
            return IsFacingRight && horizontalDirection < 0 || !IsFacingRight && horizontalDirection > 0;
        }
    
        private void Flip()
        {
            IsFacingRight = !IsFacingRight;

            var localScale = _references.body.localScale;
            localScale.x *= -1f;

            _references.body.localScale = localScale;
        }
    }
}