using System;
using System.Collections;
using Characters.Interfaces;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Characters.Behaviours
{
    public class AttackBehaviour : IAttackable, ITickable
    {
        [Serializable]
        public class Settings
        {
            //TODO: Move to Item class
            public int damage;
            public float range;
            public float attackTime;
            public float nextAttackDelay;
        }
        
        [Serializable]
        public class References
        {
            public Transform handPoint;
            public Transform weapon;
            public TrailRenderer slashEffect;
            public AudioSource audioSource;
            public AudioClip hitSound;
            public AudioClip missSound;
            public Transform hitPoint;
        }

        [SerializeField] private LayerMask layerMask;
    
        private References _references;
        private Settings _settings;
        private Transform _lookAt;

        private bool _isAnimation;
        private bool _reversedAttack;
        private bool _canAttack = true;

        [Inject]
        public void Construct(References references, Settings settings)
        {
            _references = references;
            _settings = settings;
        }

        public void Tick()
        {
            if (!_isAnimation && _lookAt != null)
                CalculateHandDirection();
        }

        public void Attack()
        {
            if (_isAnimation || !_canAttack)
                return;
        
            SwordAnimation();
        
            _reversedAttack = !_reversedAttack;
        }

        public void SetTarget(Transform target)
        {
            _lookAt = target;
        }
    
        private void CalculateHandDirection()
        {
            var direction = GetDirectionToTarget();
            handPoint.up = direction;
        }

        private Vector2 GetDirectionToTarget()
        {
            var targetPosition = _lookAt.position;
            var handPosition = transform.position;
            var direction = new Vector2(handPosition.x - targetPosition.x, handPosition.y - targetPosition.y);

            return direction;
        }

        private void SwordAnimation()
        {
            _isAnimation = true;

            var direction = _reversedAttack ? -1 : 1;
            var newRotation = new Vector3(0, 0, 180 * direction);

            StartCoroutine(StartHitting());

            _references.handPoint.DOLocalRotate(newRotation, _settings.attackTime, RotateMode.FastBeyond360)
                .SetEase(Ease.InCubic)
                .SetRelative(true)
                .OnStart(() => slashEffect.emitting = true)
                .OnComplete(() =>
                {
                    _references.slashEffect.emitting = false;
                    _isAnimation = false;
                });
        
            var newWeaponRotation = new Vector3(0, 0, 100 * direction);
            _references.weapon.DOLocalRotate(newWeaponRotation, _settings.attackTime, RotateMode.FastBeyond360)
                .SetEase(Ease.InCubic)
                .SetRelative(true);
        }

        private IEnumerator StartHitting()
        {
            _canAttack = false;
            
            yield return new WaitForSeconds(_settings.attackTime / 1.5f);
        
            _references.slashEffect.emitting = true;
            var results = Physics2D.OverlapCircleAll(_references.hitPoint.position, _settings.range, layerMask);

            var hitAnything = results.Length > 0;
            PlaySound(hitAnything);

            foreach (var result in results)
            {
                var hittable = result.transform.GetComponent<IHittable>();
                if (hittable == null || hittable.Handler == transform)
                    continue;
            
                hittable.Hit(_settings.damage);
            }

            yield return new WaitForSeconds(_settings.attackTime / 3f + _settings.nextAttackDelay);

            _canAttack = true;
        }

        private void PlaySound(bool hitAnything)
        {
            if (_references.audioSource == null)
                return;
            
            _references.audioSource.PlayOneShot(hitAnything ? _references.hitSound : _references.missSound);
        }
    }
}