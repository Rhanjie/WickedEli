using System;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Entities.Characters.Interfaces;
using Entities.Characters.Players;
using Map;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

namespace Entities
{
    public class StaticEntity : IsometricObject, IHittable, IDestroyable
    {
        private int _currentHealth;
        private bool _isInsensitive;
        
        protected References EntityReferences;
        protected Settings EntitySettings;

        public UnityAction<int> OnHealthChanged;
        
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

        public virtual void Destroy()
        {
            gameObject.SetActive(false);
        }

        public void Hit(int damage)
        {
            if (_isInsensitive || !EntitySettings.hittable)
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

            var multiplier = EntitySettings.scaleMultiplier;
            transform.localScale *= Random.Range(multiplier.x, multiplier.y);
        }

        private void HitAnimation()
        {
            if (EntityReferences.audioSource != null && !EntityReferences.audioSource.isPlaying)
                EntityReferences.audioSource.PlayOneShot(EntitySettings.hitSound);

            EntityReferences.body.DOColor(Color.red, EntitySettings.insensitivityTime)
                .SetLoops(2, LoopType.Yoyo)
                .OnStart(() => _isInsensitive = true)
                .OnComplete(() => _isInsensitive = false);
        }

        [Serializable]
        public class Settings
        {
            [Title("General")]
            public string title;
            public string translationTag;
            public Sprite sprite;

            [Title("Destroyable")]
            public bool hittable = true;
            [ShowIf("hittable")] public int health = 12; //3 hearts * 4 pieces
            [ShowIf("hittable")] public float insensitivityTime = 1f;
            [ShowIf("hittable")] public AudioClip hitSound;
            
            [Space]
            public float visionRange = 10f;

            [Title("Appearance [Optional]")]
            public Color color = Color.white;
            [MinMaxSlider(0.1f, 5f, true)]
            public Vector2 scaleMultiplier = new(1, 1);
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