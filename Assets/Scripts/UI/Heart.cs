using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Heart : MonoBehaviour
    {
        public const int MaxPieces = 4;

        [SerializeField] private Image background;

        [SerializeField] private Image image;

        public int CurrentPieces { get; private set; }

        public void Init(Vector2 position)
        {
            transform.localPosition = position;
        }

        public int AddPieces(int pieces)
        {
            //2 + 10 = 12 - 4 = 8
            //2 + 1 = 3 - 4 = -1

            if (pieces <= 0)
                return 0;

            var remaining = CurrentPieces + pieces - MaxPieces;
            CurrentPieces = MaxPieces;
            if (remaining <= 0)
            {
                CurrentPieces += remaining;
                remaining = 0;
            }

            UpdateImage();
            return remaining;
        }

        public int RemovePieces(int pieces)
        {
            //2 - 10 = -8 remaining
            //2 - 3 = -1

            if (pieces <= 0)
                return 0;

            var remaining = CurrentPieces - pieces;
            if (remaining >= 0)
            {
                CurrentPieces = remaining;
                remaining = 0;
            }

            else
            {
                CurrentPieces = 0;
            }

            UpdateImage();
            return -remaining;
        }

        private void UpdateImage()
        {
            image.fillAmount = CurrentPieces / 4f;
        }
    }
}