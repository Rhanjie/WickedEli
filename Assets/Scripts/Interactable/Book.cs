using Entities;
using Entities.Characters.Players;
using UI;
using UnityEngine;
using Zenject;

namespace Interactable
{
    public class Book : MonoBehaviour, IInteractable
    {
        [SerializeField] private string title;
        [SerializeField] private string content;

        private HUD _hud;

        [Inject]
        private void Construct(HUD hud)
        {
            _hud = hud;
        }

        public void Interact(LivingEntity livingEntity)
        {
            if (livingEntity is not Player)
                return;
            
            _hud.OpenDictionary(title, content);
        }
    }
}