using Characters;
using Characters.Players;
using UnityEngine;

namespace Interactable
{
    public class Book : MonoBehaviour, IInteractable
    {
        [SerializeField] private string title;

        [SerializeField] private string content;

        public void Interact(LivingEntity livingEntity)
        {
            if (livingEntity is Player player)
                player.OpenDictionary(title, content);
        }
    }
}