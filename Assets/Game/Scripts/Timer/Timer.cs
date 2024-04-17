using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.Timer
{
    public class Timer : MonoBehaviour
    {
        public event Action<int> OnTimerUpdate;
        public event Action OnTimeIsUp;

        private DateTime _targetTime;
        private CancellationTokenSource _cts;
        private TimeSpan _currentRemainingTime;
        private int _previousRemainingSeconds;

        public void StartTimer(int duration)
        {
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            _targetTime = DateTime.Now.AddSeconds(duration);
            _previousRemainingSeconds = duration;
            TickTimer().Forget();
        }

        public void CancelTimer()
        {
            _cts?.Cancel();
        }

        private async UniTask TickTimer()
        {
            while (true)
            {
                await UniTask.NextFrame();
                
                if (_cts.IsCancellationRequested)
                {
                    break;
                }

                _currentRemainingTime = _targetTime - DateTime.Now;
                var currentRemainingTotalSeconds = Mathf.RoundToInt((float)_currentRemainingTime.TotalSeconds);
                if (_previousRemainingSeconds == currentRemainingTotalSeconds)
                {
                    return;
                }

                _previousRemainingSeconds = currentRemainingTotalSeconds;
                OnTimerUpdate?.Invoke(currentRemainingTotalSeconds);
                
                if (currentRemainingTotalSeconds > 0)
                {
                    continue;
                }
                
                OnTimeIsUp?.Invoke();
                break;
            }
        }
    }
}