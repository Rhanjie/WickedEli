using UnityEngine;

namespace Characters.Interfaces
{
    public class Book : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private string title;
        
        [SerializeField]
        private string content;

        public void Interact(Character character)
        {
            if (character is Player player)
                player.OpenDictionary(title, content);
        }
    }
}