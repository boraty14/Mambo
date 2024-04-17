using System;
using Game.Scripts.Binding;

namespace Game.Scripts.Timer
{
    public interface ITimer : IService
    {
        void StartTimer(int duration);
        void CancelTimer();
        
        event Action<int> OnTimerUpdate;
        event Action OnTimeIsUp;

    }
}