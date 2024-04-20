namespace Game.Scripts.UI.GameUI
{
    public class GameUIView : ViewBase
    {
        private void OnEnable()
        {
            EventBus.OnStartGame += OnStartGame;
            EventBus.OnTimeIsUp += OnTimeIsUp;
        }

        private void OnDisable()
        {
            EventBus.OnStartGame -= OnStartGame;
            EventBus.OnTimeIsUp -= OnTimeIsUp;
        }

        private void OnStartGame()
        {
            ToggleView(true);
        }

        private void OnTimeIsUp()
        {
            ToggleView(false);
        }
    }
}