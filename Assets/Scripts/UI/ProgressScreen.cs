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
        }

        private void OnDisable()
        {
            TerrainGenerator.OnProgressUpdate -= ProgressAnimation;
            TerrainGenerator.OnProgressFinished -= HideProgressScreen;
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
