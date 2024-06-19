using System;
using Characters.Interfaces;
using DG.Tweening;
using Map;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Characters
{
    public class StaticEntity : IsometricObject, IHittable, IDestroyable
    {
        private int _currentHealth;
        private bool _isInsensitive;
        
        protected References EntityReferences;
        protected Settings EntitySettings;

        public UnityAction<int> OnHealthChanged;

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

        public virtual void Destroy()
        {
            gameObject.SetActive(false);
        }

        public Transform Handler { get; protected set; }

        public void Hit(int damage)
        {
            if (_isInsensitive)
                return;

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                Destroy();

            else HitAnimation();
        }

        [Inject]
        public void Construct(Settings settings, References references)
        {
            EntitySettings = settings;
            EntityReferences = references;

            Handler = transform;
            CurrentHealth = EntitySettings.health;
        }

        private void HitAnimation()
        {
            if (EntityReferences.audioSource != null && !EntityReferences.audioSource.isPlaying)
                EntityReferences.audioSource.Play();

            EntityReferences.body.DOColor(Color.black, EntitySettings.insensitivityTime)
                .SetLoops(2, LoopType.Yoyo)
                .OnStart(() => _isInsensitive = true)
                .OnComplete(() => _isInsensitive = false);
        }

        [Serializable]
        public class Settings
        {
            public string title;
            public int health;
            public float insensitivityTime;
            public float range;
        }

        [Serializable]
        public new class References
        {
            public SpriteRenderer body;
            public Animator animator;
            public AudioSource audioSource;
            public Transform lookAt;
        }
    }
}