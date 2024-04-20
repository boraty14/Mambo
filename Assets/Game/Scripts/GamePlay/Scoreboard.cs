using System;
using UnityEngine;

namespace Game.Scripts.GamePlay
{
    public class Scoreboard : MonoBehaviour
    {
        public event Action<int> OnScoreUpdate;
        private int _currentScore;
        
        private void OnEnable()
        {
            EventBus.OnBlast += OnBlast;
        }

        private void OnDisable()
        {
            EventBus.OnBlast -= OnBlast;
            ResetScore();
        }

        private void OnBlast(int score)
        {
            _currentScore += score;
            OnScoreUpdate?.Invoke(_currentScore);
        }
        
        public int GetScore()
        {
            return _currentScore;
        }

        private void ResetScore()
        {
            _currentScore = 0;
            OnScoreUpdate?.Invoke(_currentScore);
        }
    }
}