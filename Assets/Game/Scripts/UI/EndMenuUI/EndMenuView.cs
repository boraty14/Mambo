using Game.Scripts.GamePlay;
using UnityEngine;

namespace Game.Scripts.UI.EndMenuUI
{
    public class EndMenuView : ViewBase
    {
        [SerializeField] private EndScoreView _endScoreView;
        
        private void OnEnable()
        {
            EventBus.OnReturnMainMenu += OnReturnMainMenu;
            EventBus.OnEndGame += OnEndGame;
        }

        private void OnDisable()
        {
            EventBus.OnReturnMainMenu -= OnReturnMainMenu;
            EventBus.OnEndGame -= OnEndGame;
        }

        private void OnReturnMainMenu()
        {
            ToggleView(false);
        }

        private void OnEndGame(GameData gameData)
        {
            ToggleView(true);
        }
    }
}