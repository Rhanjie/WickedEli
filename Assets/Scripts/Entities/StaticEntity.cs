﻿using System;
using DG.Tweening;
using Entities.Characters.Interfaces;
using Map;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Entities
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
            
            Init();
        }

        public virtual void Init()
        {
            if (EntitySettings.color.a == 0)
                EntitySettings.color.a = 255f;
            
            if (EntitySettings.sprite != null)
                EntityReferences.body.sprite = EntitySettings.sprite;
            
            if (EntitySettings.color != Color.clear)
                EntityReferences.body.color = EntitySettings.color;

            transform.localScale *= EntitySettings.scaleMultiplier;
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
            
            [Title("Appearance [Optional]")]
            public Sprite sprite;
            public Color color = Color.white;
            public float scaleMultiplier = 1;
        }

        [Serializable]
        public new class References
        {
            public SpriteRenderer body;
            public AudioSource audioSource;
            
            [Title("Optional")]
            public Animator animator;
        }
    }
}