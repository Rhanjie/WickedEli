using Characters.Behaviours;
using Characters.Interfaces;
using Characters.Settings;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public abstract class Character : IsometricObject, IHittable, IDestroyable
    {
        [SerializeField]
        protected CharacterSettings settings;
    
        [SerializeField]
        protected MovementBehaviour movement;
    
        [SerializeField]
        protected AttackBehaviour attack;
    
        [SerializeField]
        protected SpriteRenderer body;
    
        [SerializeField]
        private Animator animator;
    
        [SerializeField]
        private AudioSource audioSource;
    
        [SerializeField]
        private Transform lookAt;

        public Transform LookAt
        {
            get => lookAt;
            set
            {
                lookAt = value;
            
                movement.SetTarget(lookAt);
                attack.SetTarget(lookAt);
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

        protected virtual void Start()
        {
            Handler = transform;
            CurrentHealth = settings.health;

            UpdateBehaviours();
        }
    
        private void OnValidate()
        {
            UpdateBehaviours();
        }

        private void UpdateBehaviours()
        {
            movement.UpdateSettings(settings);
            movement.SetTarget(lookAt);
        
            attack.UpdateSettings(settings);
            attack.SetTarget(lookAt);
        }
    
        protected override void Update()
        {
            base.Update();
        
            animator.SetFloat(Velocity, movement.Velocity);
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
            if (audioSource != null && !audioSource.isPlaying)
                audioSource.Play();
        
            body.DOColor(Color.black, settings.insensitivityTime)
                .SetLoops(2, LoopType.Yoyo)
                .OnStart(() => _isInsensitive = true)
                .OnComplete(() => _isInsensitive = false);
        }

        public abstract void Destroy();
    }
}