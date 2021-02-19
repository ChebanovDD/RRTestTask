using Core.ViewModels;
using UnityEngine;

namespace Core
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private SplashScreenViewModel _splashScreen;
        [SerializeField] private GameBoardViewModel _gameBoard;
        [SerializeField] private GameOverScreenViewModel _gameOverScreen;

        public SplashScreenViewModel SplashScreen => _splashScreen;
        public GameBoardViewModel GameBoard => _gameBoard;
        public GameOverScreenViewModel GameOverScreen => _gameOverScreen;
    }
}
