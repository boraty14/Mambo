using UnityEngine;

namespace Game.Scripts.GamePlay
{
    public class Scoreboard : MonoBehaviour
    {
        private int _currentScore;
        
        private void OnEnable()
        {
            EventBus.OnStartGame += OnStartGame;
            EventBus.OnCalculateScore += OnCalculateScore;
        }

        private void OnDisable()
        {
            EventBus.OnStartGame -= OnStartGame;
            EventBus.OnCalculateScore -= OnCalculateScore;
        }

        private void OnStartGame()
        {
            EventBus.OnBlast += OnBlast;
        }

        private void OnCalculateScore()
        {
            EventBus.OnBlast -= OnBlast;
            
            var highScore = PlayerSave.GetHighScore();
            var isHighScore = _currentScore > highScore;
            var gameData = new GameData()
            {
                IsHighScore = isHighScore,
                Score = _currentScore
            };
            
            if (isHighScore)
            {
                PlayerSave.SetHighScore(_currentScore);
            }
            
            _currentScore = 0;
            EventBus.EndGame(gameData);
        }

        private void OnBlast(int score)
        {
            _currentScore += score;
            EventBus.UpdateScore(_currentScore);
        }
        
        public int GetScore()
        {
            return _currentScore;
        }
    }
}