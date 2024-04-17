using System;
using UnityEngine;

namespace Game.Scripts.Score
{
    public class Scoreboard : MonoBehaviour
    {
        public event Action<int> OnScoreUpdate;

        private int _currentScore;
        
        public void AddScore(int amount)
        {
            _currentScore += amount;
            OnScoreUpdate?.Invoke(_currentScore);
        }

        public int GetScore()
        {
            return _currentScore;
        }

        public void ResetScore()
        {
            _currentScore = 0;
            OnScoreUpdate?.Invoke(_currentScore);
        }

    }
}