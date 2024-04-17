using System;
using Game.Scripts.Binding;

namespace Game.Scripts.Score
{
    public interface IScoreboard : IService
    {
        void AddScore(int amount);
        int GetScore();
        void ResetScore();

        event Action<int> OnScoreUpdate;

    }
}