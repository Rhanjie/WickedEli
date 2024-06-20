using System;
using System.Linq;
using Interactable;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

namespace Characters.Players
{
    public class Player : LivingEntity
    {
        protected Settings PlayerSettings;
        
        private IInteractable _target;
        private Camera _mainCamera;
        private HUD _hud;

        [Inject]
        private void Construct(Settings settings, Camera mainCamera, HUD hud)
        {
            PlayerSettings = settings;
            _mainCamera = mainCamera;
            _hud = hud;

            OnHealthChanged += _hud.UpdateHealth;
        }

        protected override void Update()
        {
            base.Update();

            UpdateTargetPosition();

            _target = FindInteractableObject();
            ToggleInteraction(_target);
            
            InteractionListener();
        }

        public override void Destroy()
        {
            //TODO: Gameover

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
            OnHealthChanged -= _hud.UpdateHealth;
        }

        public void PerformMove(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();

            MovementBehaviour.Move(delta);
        }

        public void PerformAttack()
        {
            AttackBehaviour.Attack();
        }

        private IInteractable FindInteractableObject()
        {
            var position = EntityReferences.body.transform.position;
            var layerMask = LayerMask.GetMask("Interactable");
            
            var target = Physics2D.OverlapCircle(position, 5f, layerMask);
            if (target == null)
                return null;
            
            return target.GetComponent<IInteractable>();
        }

        private void ToggleInteraction(IInteractable interactable)
        {
            var foundInteractable = interactable != null;
            _hud.ToggleInteractionText(foundInteractable);
        }

        private void InteractionListener()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (_target == null)
                    return;

                _target.Interact(this);
            }
        }

        private void UpdateTargetPosition()
        {
            if (AttackBehaviour.LookAt == null)
                return;
            
            var mousePosition = Mouse.current.position;
            var convertedPosition = _mainCamera.ScreenToWorldPoint(mousePosition.value);

            AttackBehaviour.LookAt.transform.position = convertedPosition;
        }

        [Serializable]
        public new class Settings
        {
            public int test;
        }
    }
}