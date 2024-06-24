using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Text interactionText;

        [SerializeField] private Dictionary dictionary;

        [SerializeField] private HeartContainer heartContainer;

        public void Init(int health)
        {
            var additionalHeart = health % 4 != 0 ? 1 : 0;
            var hearts = health / 4 + additionalHeart;
            
            AddMaxHealth(hearts, false);
            UpdateHealth(health);
        }

        public void UpdateHealth(int difference)
        {
            if (difference > 0)
                heartContainer.AddHealth(difference);

            else heartContainer.RemoveHealth(-difference);
        }

        public void AddMaxHealth(int hearts, bool fill)
        {
            heartContainer.AddMaxHealth(hearts, fill);
        }

        public void ToggleInteractionText(bool value)
        {
            if (interactionText.gameObject.activeSelf != value)
                interactionText.gameObject.SetActive(value);
        }

        public void OpenDictionary(string title, string content)
        {
            dictionary.Open(title, content);
        }

        public void CloseDictionary()
        {
            dictionary.Close();
        }
    }
}