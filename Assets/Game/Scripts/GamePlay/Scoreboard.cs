using UnityEngine;

namespace Game.Scripts.GamePlay
{
    public class Scoreboard : MonoBehaviour
    {
        private int _currentScore;
        
        private void OnEnable()
        {
            EventBus.OnBlast += OnBlast;
            EventBus.OnTimeIsUp += OnTimeIsUp;
        }

        private void OnDisable()
        {
            EventBus.OnBlast -= OnBlast;
            EventBus.OnTimeIsUp -= OnTimeIsUp;
        }

        private void OnTimeIsUp()
        {
            var highScore = PlayerSave.GetHighScore();
            var isHighScore = _currentScore > highScore;
            var gameData = new GameData()
            {
                IsHighScore = isHighScore,
                Score = _currentScore
            };
            
            if (_currentScore <= highScore)
            {
                PlayerSave.SetHighScore(highScore);
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