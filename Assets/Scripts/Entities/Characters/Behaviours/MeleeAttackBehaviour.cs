using System;
using System.Collections.Generic;
using DG.Tweening;
using Entities.Characters.Interfaces;
using MEC;
using UnityEngine;
using Zenject;

namespace Entities.Characters.Behaviours
{
    public class MeleeAttackBehaviour : IAttackBehaviour, ITickable
    {
        private Transform _handler;
        private References _references;
        private Settings _settings;
        
        private bool _canAttack = true;
        private bool _isAnimation;
        private bool _reversedAttack;

        public Transform LookAt { get; set; }

        public void Attack()
        {
            if (_isAnimation || !_canAttack)
                return;

            SwordAnimation();

            _reversedAttack = !_reversedAttack;
        }

        public void Tick()
        {
            if (!_isAnimation && LookAt != null)
                CalculateHandDirection();
        }

        [Inject]
        public void Construct(Transform handler, References references, Settings settings)
        {
            _handler = handler;
            _references = references;
            _settings = settings;
            
            LookAt = references.lookAt;
        }

        private void CalculateHandDirection()
        {
            var direction = GetDirectionToTarget();
            _references.handPoint.up = direction;
        }

        private Vector2 GetDirectionToTarget()
        {
            var targetPosition = LookAt.position;
            var handPosition = _handler.position;
            var direction = new Vector2(handPosition.x - targetPosition.x, handPosition.y - targetPosition.y);

            return direction;
        }

        private void SwordAnimation()
        {
            _isAnimation = true;

            var direction = _reversedAttack ? -1 : 1;
            var newRotation = new Vector3(0, 0, 180 * direction);

            Timing.RunCoroutine(StartHitting());
            
            _references.weapon.DOLocalRotate(newRotation, _settings.attackTime, RotateMode.FastBeyond360)
                .OnStart(() => _references.slashEffect.emitting = true)
                .SetEase(Ease.InQuint)
                .SetRelative(true);
            
            _references.handPoint.DOLocalRotate(newRotation, _settings.attackTime, RotateMode.FastBeyond360)
                .SetDelay(0.1f)
                .SetEase(Ease.InQuint)
                .SetRelative(true)
                .OnComplete(() =>
                {
                    _references.slashEffect.emitting = false;
                    Timing.RunCoroutine(LateOnAnimationCompleted(0.2f));
                });
        }

        private IEnumerator<float> LateOnAnimationCompleted(float delay)
        {
            yield return Timing.WaitForSeconds(delay);
            
            _isAnimation = false;
        }

        private IEnumerator<float> StartHitting()
        {
            _canAttack = false;

            yield return Timing.WaitForSeconds(_settings.attackTime / 1.5f);

            _references.slashEffect.emitting = true;
            var results =
                Physics2D.OverlapCircleAll(_references.hitPoint.position, _settings.range, _settings.layerMask);

            var hitAnything = results.Length > 0;
            PlaySound(hitAnything);

            foreach (var result in results)
            {
                var hittable = result.transform.GetComponent<IHittable>();
                if (hittable == null || hittable.Handler == _handler)
                    continue;

                hittable.Hit(_settings.damage);
            }

            yield return Timing.WaitForSeconds(_settings.attackTime / 3f + _settings.nextAttackDelay);

            _canAttack = true;
        }

        private void PlaySound(bool hitAnything)
        {
            if (_references.audioSource == null)
                return;

            _references.audioSource.PlayOneShot(hitAnything ? _settings.hitSound : _settings.missSound);
        }

        [Serializable]
        public class Settings
        {
            //TODO: Move to Item class
            public int damage;
            public float range;
            public float attackTime;
            public float nextAttackDelay;

            public LayerMask layerMask;
            public AudioClip hitSound;
            public AudioClip missSound;
        }

        [Serializable]
        public class References
        {
            public Transform handPoint;
            public Transform weapon;
            public TrailRenderer slashEffect;
            public AudioSource audioSource;
            public Transform hitPoint;
            public Transform lookAt;
        }
    }
}