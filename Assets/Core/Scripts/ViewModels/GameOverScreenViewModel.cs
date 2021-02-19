using UnityEngine;

namespace Core.ViewModels
{
    public class GameOverScreenViewModel : MonoBehaviour
    {
        public void Area_Click()
        {
            QuitApplication();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void QuitApplication()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
