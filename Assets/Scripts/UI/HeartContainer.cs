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

        public void Update()
        {
            //TODO: Only test
            if (Application.isEditor)
            {
                if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
                    AddHealth(1);
                
                if (Keyboard.current.leftAltKey.wasPressedThisFrame)
                    AddMaxHealth(1, false);

                if (Keyboard.current.leftCtrlKey.wasPressedThisFrame)
                    RemoveHealth(1);
            }
        }

        public void AddHealth(int health)
        {
            if (health <= 0)
                return;
            
            for (var i = 0; i < _hearts.Count; i++)
            {
                if (_hearts[i].CurrentPieces == Heart.MaxPieces)
                    continue;
                
                health = _hearts[i].AddPieces(health);
            }
        }

        public void AddMaxHealth(int hearts, bool fill)
        {
            for (var i = 0; i < hearts; i++)
                SpawnNewHeart();

            if (fill)
                AddHealth(hearts * 4);
        }

        public void RemoveHealth(int health)
        {
            if (health <= 0 || _hearts.Count == 0)
                return;
            
            for (var i = _hearts.Count - 1; i >= 0; i--)
            {
                if (_hearts[i].CurrentPieces == 0)
                    continue;
                
                health = _hearts[i].RemovePieces(health);
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