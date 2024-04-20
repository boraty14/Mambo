using System;

namespace Game.Scripts.UI.MainMenuUI
{
    public class MainMenuView : ViewBase
    {
        private void OnEnable()
        {
            EventBus.OnStartGame += OnStartGame;
            EventBus.OnReturnMainMenu += OnReturnMainMenu;

        }

        private void OnDisable()
        {
            EventBus.OnStartGame -= OnStartGame;
            EventBus.OnReturnMainMenu -= OnReturnMainMenu;
        }

        private void OnStartGame()
        {
            ToggleView(false);
        }

        private void OnReturnMainMenu()
        {
            ToggleView(true);
        }
    }
}