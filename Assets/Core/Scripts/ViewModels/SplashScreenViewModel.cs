using DG.Tweening;
using UnityEngine;

namespace Core.ViewModels
{
    public class SplashScreenViewModel : MonoBehaviour
    {
        [SerializeField] private ProgressBar _progressBar;

        [Header("Animation")]
        [SerializeField] private float _duration = 0.4f;
        [SerializeField] private Ease _easeType;

        private RectTransform _rectTransform;

        public ProgressBar ProgressBar => _progressBar;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Hide()
        {
            transform.DOMoveY(_rectTransform.rect.height * 2, _duration).SetEase(_easeType)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}
