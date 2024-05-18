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
        private IInteractable _target;
        protected References PlayerReferences;

        protected Settings PlayerSettings;

        protected override void Update()
        {
            base.Update();

            UpdateTargetPosition();

            InteractionChecker();
            InteractionListener();
        }

        private void OnDestroy()
        {
            OnHealthChanged -= PlayerReferences.hud.UpdateHealth;
        }

        [Inject]
        private void Construct(Settings settings, References references)
        {
            PlayerSettings = settings;
            PlayerReferences = references;

            OnHealthChanged += PlayerReferences.hud.UpdateHealth;
        }

        private void InteractionChecker()
        {
            var position = EntityReferences.body.transform.position;
            var size = new Vector2(3, 5);
            var layerMask = LayerMask.GetMask("Interactable");

            var results = Physics2D.OverlapBoxAll(position, size, layerMask).ToList();

            var interactables = results
                .Select(result => result.GetComponent<IInteractable>())
                .ToArray();

            if (interactables.Length == 0)
            {
                ClearInteraction();

                return;
            }

            var foundTarget = interactables.First();
            if (foundTarget == _target)
                return;

            _target = foundTarget;

            PlayerReferences.hud.ToggleInteractionText(true);
        }

        private void ClearInteraction()
        {
            PlayerReferences.hud.ToggleInteractionText(false);
            _target = null;
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

        public void PerformMove(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();

            MovementBehaviour.Move(delta);
        }

        public void PerformAttack()
        {
            AttackBehaviour.Attack();
        }

        public override void Destroy()
        {
            //TODO: Gameover

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OpenDictionary(string title, string content)
        {
            PlayerReferences.hud.OpenDictionary(title, content);
        }

        private void UpdateTargetPosition()
        {
            var mousePosition = Mouse.current.position;
            var convertedPosition = PlayerReferences.mainCamera.ScreenToWorldPoint(mousePosition.value);

            LookAt.transform.position = convertedPosition;
        }

        [Serializable]
        public new class Settings
        {
            public int test;
        }

        [Serializable]
        public new class References
        {
            public HUD hud;
            public Camera mainCamera;
        }
    }
}