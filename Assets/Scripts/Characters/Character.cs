using System;
using Characters.Behaviours;
using Characters.Interfaces;
using DG.Tweening;
using Sirenix.OdinInspector;
using Terrain;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Characters
{
    public abstract class Character : IsometricObject, IHittable, IDestroyable
    {
        [Serializable]
        public class Settings
        {
            [Title("General")]
            public string title;
    
            [Title("Attack")]
            public int health;
            public int damage;
            public float range;
            public float insensitivityTime;
            public float attackTime;
            public float nextAttackDelay;
    
            [Title("Movement")]
            public float speed;
            public float acceleration;
            public float friction;
        }

        [Serializable]
        public new class References
        {
            public MovementBehaviour movement;
            public AttackBehaviour attack;
            public SpriteRenderer body;
            public Animator animator;
            public AudioSource audioSource;
            public Transform lookAt;
        }

        [Inject]
        public void Construct(Settings settings, References references)
        {
            CharacterSettings = settings;
            CharacterReferences = references;
            
            Handler = transform;
            CurrentHealth = CharacterSettings.health;

            UpdateBehaviours();
        }

        protected Settings CharacterSettings;
        protected References CharacterReferences;

        public Transform LookAt
        {
            get => CharacterReferences.lookAt;
            set
            {
                CharacterReferences.lookAt = value;
            
                CharacterReferences.movement.SetTarget(value);
                CharacterReferences.attack.SetTarget(value);
            }
        }

        public Transform Handler { get; protected set; }

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                var difference = value - _currentHealth;
                OnHealthChanged?.Invoke(difference);
                _currentHealth = value;
            }
        }

        public UnityAction<int> OnHealthChanged;

        private int _currentHealth;
        private bool _isInsensitive;
    
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        
        private void UpdateBehaviours()
        {
            CharacterReferences.movement.UpdateSettings(CharacterSettings);
            CharacterReferences.movement.SetTarget(CharacterReferences.lookAt);
        
            CharacterReferences.attack.UpdateSettings(CharacterSettings);
            CharacterReferences.attack.SetTarget(CharacterReferences.lookAt);
        }
    
        protected override void Update()
        {
            base.Update();
        
            CharacterReferences.animator.SetFloat(Velocity, CharacterReferences.movement.Velocity);
        }

        public void Hit(int damage)
        {
            if (_isInsensitive)
                return;

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                Destroy();

            else HitAnimation();
        }

        private void HitAnimation()
        {
            if (CharacterReferences.audioSource != null && !CharacterReferences.audioSource.isPlaying)
                CharacterReferences.audioSource.Play();
        
            CharacterReferences.body.DOColor(Color.black, CharacterSettings.insensitivityTime)
                .SetLoops(2, LoopType.Yoyo)
                .OnStart(() => _isInsensitive = true)
                .OnComplete(() => _isInsensitive = false);
        }

        public abstract void Destroy();
    }
}