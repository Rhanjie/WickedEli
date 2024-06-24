using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        //TODO: Add injections and refactor
        
        [SerializeField] private CanvasGroup mainGroup;
        [SerializeField] private CanvasGroup settingsGroup;
        [SerializeField] private Slider volumeSlider;

        private Coroutine _coroutine;

        private void Start()
        {
            volumeSlider.SetValueWithoutNotify(AudioListener.volume);
        }

        public void StartGame()
        {
            if (_coroutine != null)
                return;
            
            StartCoroutine(StartGameRoutine());
        }

        private IEnumerator<float> StartGameRoutine()
        {
            var loadOperation = SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Single);
            yield return Timing.WaitUntilDone(loadOperation);

            _coroutine = null;
        }

        public void GoToSettings()
        {
            ToggleCanvasGroup(mainGroup, false);
            ToggleCanvasGroup(settingsGroup, true);
        }

        public void BackToMainMenu()
        {
            ToggleCanvasGroup(mainGroup, true);
            ToggleCanvasGroup(settingsGroup, false);
        }

        public void SetVolume(float value)
        {
            AudioListener.volume = value;
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void ToggleCanvasGroup(CanvasGroup canvasGroup, bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }
    }
}