using System;
using Map;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressScreen : MonoBehaviour
    {
        [SerializeField] private Image loaderProgress;

        private void OnEnable()
        {
            TerrainGenerator.OnProgressUpdate += ProgressAnimation;
            TerrainGenerator.OnProgressFinished += HideProgressScreen;
            
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            TerrainGenerator.OnProgressUpdate -= ProgressAnimation;
            TerrainGenerator.OnProgressFinished -= HideProgressScreen;
            
            Time.timeScale = 1f;
        }
        
        private void ProgressAnimation(float progress)
        {
            loaderProgress.fillAmount = progress;
        }

        private void HideProgressScreen()
        {
            gameObject.SetActive(false);
        }
    }
}
