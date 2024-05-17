using System;
using Characters.Interfaces;
using DG.Tweening;
using Sirenix.OdinInspector;
using Terrain;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Characters
{
    public abstract class StaticEntity : IsometricObject, IHittable, IDestroyable
    {
        [Serializable]
        public class Settings
        {
            public string title;
            public int health;
            public float insensitivityTime;
        }
        
        [Serializable]
        public new class References
        {
            public SpriteRenderer body;
            public Animator animator;
            public AudioSource audioSource;
            public Transform lookAt;
        }

        protected Settings EntitySettings;
        protected References EntityReferences;
        
        public UnityAction<int> OnHealthChanged;

        private int _currentHealth;
        private bool _isInsensitive;
        
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
        
        [Inject]
        public void Construct(Settings settings, References references)
        {
            EntitySettings = settings;
            EntityReferences = references;
            
            Handler = transform;
            CurrentHealth = EntitySettings.health;
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
            if (EntityReferences.audioSource != null && !EntityReferences.audioSource.isPlaying)
                EntityReferences.audioSource.Play();
        
            EntityReferences.body.DOColor(Color.black, EntitySettings.insensitivityTime)
                .SetLoops(2, LoopType.Yoyo)
                .OnStart(() => _isInsensitive = true)
                .OnComplete(() => _isInsensitive = false);
        }

        public abstract void Destroy();
    }
}