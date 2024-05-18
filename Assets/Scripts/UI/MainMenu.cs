using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup mainGroup;
    
        [SerializeField]
        private CanvasGroup settingsGroup;
    
        [SerializeField]
        private AudioSource audioSource;
    
        [SerializeField]
        private Slider volumeSlider;

        private void Start()
        {
            volumeSlider.SetValueWithoutNotify(AudioListener.volume);
        }

        public void StartGame()
        {
            SceneManager.LoadScene("Gameplay");
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
        
            audioSource.Play();
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
