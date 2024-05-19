using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Dictionary : MonoBehaviour
    {
        [SerializeField] private Text titleObject;
        [SerializeField] private TextAnimator textAnimator;
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private float transitionTime;

        public void Open(string title, string content)
        {
            titleObject.text = title;

            canvasGroup.DOFade(1, transitionTime)
                .OnStart(() => textAnimator.Init(content))
                .OnComplete(() => { canvasGroup.interactable = canvasGroup.blocksRaycasts = true; });
        }

        public void Close()
        {
            canvasGroup.DOFade(0, transitionTime).OnComplete(() =>
            {
                canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
            });
        }
    }
}