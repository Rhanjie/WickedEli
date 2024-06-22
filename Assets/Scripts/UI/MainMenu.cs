using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        //TODO: Add injections and refactor
        
        [SerializeField] private CanvasGroup mainMenu;
        [SerializeField] private CanvasGroup mainGroup;
        [SerializeField] private CanvasGroup settingsGroup;
        [SerializeField] private Slider volumeSlider;
        
        [SerializeField] private Image loaderProgress;

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

        private IEnumerator StartGameRoutine()
        {
            mainMenu.alpha = 0;
            mainMenu.interactable = mainMenu.blocksRaycasts = false;
            
            StartCoroutine(ProgressAnimation(0.5f));
            
            yield return SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);

            _coroutine = null;
        }

        private IEnumerator ProgressAnimation(float speed)
        {
            loaderProgress.fillAmount = 0;
            
            while (loaderProgress.fillAmount < 1)
            {
                loaderProgress.fillAmount += Time.deltaTime * speed;
                yield return null;
            }

            loaderProgress.fillAmount = 1;
            
            yield return SceneManager.UnloadSceneAsync("MainMenu");
            
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