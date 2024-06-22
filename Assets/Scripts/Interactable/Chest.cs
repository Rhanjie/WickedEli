using Entities;
using Entities.Characters.Players;
using UnityEngine;

namespace Interactable
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private int health;

        public void Interact(LivingEntity livingEntity)
        {
            if (livingEntity is Player player)
            {
                player.CurrentHealth += health;

                Destroy(gameObject);
            }
        }
    }
}