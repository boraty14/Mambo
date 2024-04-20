using Game.Scripts.GamePlay;

namespace Game.Scripts.UI.GameUI
{
    public class GameUIView : ViewBase
    {
        private void OnEnable()
        {
            EventBus.OnStartGame += OnStartGame;
            EventBus.OnEndGame += OnEndGame;
        }

        private void OnDisable()
        {
            EventBus.OnStartGame -= OnStartGame;
            EventBus.OnEndGame -= OnEndGame;
        }

        private void OnStartGame()
        {
            ToggleView(true);
        }

        private void OnEndGame(GameData gameData)
        {
            ToggleView(false);
        }
    }
}