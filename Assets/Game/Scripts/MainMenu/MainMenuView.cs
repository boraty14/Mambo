using UnityEngine;

namespace Game.Scripts.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private PlayButton _playButton;
        [SerializeField] private HighScoreView _highScoreView;

        private void OnEnable()
        {
            SignalBus.StartGame += OnStartGame;
        }

        private void OnDisable()
        {
            SignalBus.StartGame -= OnStartGame;
        }

        private void OnStartGame()
        {
            _playButton.Toggle(false);
            _highScoreView.Toggle(false);
        }
    }
}