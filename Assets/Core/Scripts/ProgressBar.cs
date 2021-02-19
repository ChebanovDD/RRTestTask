using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class ProgressBar : MonoBehaviour, IProgress<float>
    {
        [SerializeField] private Image _progress;

        private int _currentIteration = 1;
        private float _progressFactor = -1;

        public void SetIterationCount(int count)
        {
            if (count <= 0)
            {
                throw new InvalidOperationException("Iteration count must be greater than 0.");
            }

            _progressFactor = 1.0f / count;
        }

        public void NextIteration()
        {
            _currentIteration++;
        }

        public void Report(float value)
        {
            _progress.fillAmount = CalculateProgress(value);
        }

        public void ResetState()
        {
            _progressFactor = -1;
            _currentIteration = 1;
            _progress.fillAmount = 0;
        }

        private float CalculateProgress(float value)
        {
            if (_progressFactor < 0)
            {
                return value;
            }

            return _progressFactor * _currentIteration + value * _progressFactor / 100;
        }
    }
}
