using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class HeartContainer : MonoBehaviour
    {
        [SerializeField] private Heart heartPrefab;

        public float heartWidth;

        private readonly List<Heart> _hearts = new();

        private int _currentTestAmount = 1;

        public void Update()
        {
            //TODO: Only test
            if (Application.isEditor)
            {
                if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
                    AddHealth(_currentTestAmount++);

                if (Keyboard.current.leftCtrlKey.wasPressedThisFrame)
                    RemoveHealth(--_currentTestAmount);
            }
        }

        public void AddHealth(int health)
        {
            if (health <= 0)
                return;

            while (health > 0)
            {
                var lastHeart = _hearts.Count == 0
                    ? SpawnNewHeart()
                    : _hearts.Last();

                if (lastHeart == null || lastHeart.CurrentPieces == Heart.MaxPieces)
                    lastHeart = SpawnNewHeart();

                health = lastHeart.AddPieces(health);
            }
        }

        public void RemoveHealth(int health)
        {
            if (health <= 0 || _hearts.Count == 0)
                return;

            while (health > 0)
            {
                var lastHeart = _hearts.Last();
                if (lastHeart == null || lastHeart.CurrentPieces == 0)
                {
                    DestroyHeart(lastHeart);
                    lastHeart = _hearts.Last();
                }

                health = lastHeart.RemovePieces(health);
                if (lastHeart.CurrentPieces == 0)
                    DestroyHeart(lastHeart);
            }
        }

        private Heart SpawnNewHeart()
        {
            var heart = Instantiate(heartPrefab, transform);
            heart.Init(new Vector2(_hearts.Count * heartWidth, 0f));

            _hearts.Add(heart);
            return heart;
        }

        private void DestroyHeart(Heart heart)
        {
            if (heart == null)
                return;

            _hearts.Remove(heart);
            Destroy(heart.gameObject);
        }
    }
}