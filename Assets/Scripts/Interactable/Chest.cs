using Characters;
using Characters.Interfaces;
using Characters.Players;
using UnityEngine;

namespace Interactable
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private int health;

        public void Interact(Character character)
        {
            if (character is Player player)
            {
                player.CurrentHealth += health;
                
                Destroy(gameObject);
            }
        }
    }
}
