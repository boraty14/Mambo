using System;
using Game.Scripts.Board;
using Game.Scripts.GamePlay;

namespace Game.Scripts
{
    public static class EventBus
    {
        public static void StartGame() => OnStartGame?.Invoke();
        public static event Action OnStartGame;

        public static void SetBoardLevelData(BoardLevelData boardLevelData) => OnSetBoardLevelData?.Invoke(boardLevelData);
        public static event Action<BoardLevelData> OnSetBoardLevelData;
        
        public static void Blast(int score) => OnBlast?.Invoke(score);
        public static event Action<int> OnBlast;
        
        public static void UpdateTimer(int duration) => OnUpdateTimer?.Invoke(duration);
        public static event Action<int> OnUpdateTimer;

        public static void UpdateScore(int score) => OnUpdateScore?.Invoke(score);
        public static event Action<int> OnUpdateScore;
        
        public static void TimeIsUp() => OnTimeIsUp?.Invoke();
        public static event Action OnTimeIsUp;

        public static void CalculateScore() => OnCalculateScore?.Invoke();
        public static event Action OnCalculateScore;
        
        public static void EndGame(GameData gameData) => OnEndGame?.Invoke(gameData);
        public static event Action<GameData> OnEndGame;

        public static void ReturnMainMenu() => OnReturnMainMenu?.Invoke();
        public static event Action OnReturnMainMenu;
    }
}