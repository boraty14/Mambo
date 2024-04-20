using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Board;
using UnityEngine;

namespace Game.Scripts.GamePlay
{
    public class Timer : MonoBehaviour
    {
        private DateTime _targetTime;
        private CancellationTokenSource _cts;
        private TimeSpan _currentRemainingTime;
        private int _previousRemainingSeconds;

        private void OnEnable()
        {
            EventBus.OnSetBoardLevelData += OnSetBoardLevelData;
        }

        private void OnDisable()
        {
            EventBus.OnSetBoardLevelData -= OnSetBoardLevelData;
        }

        private void OnSetBoardLevelData(BoardLevelData boardLevelData)
        {
            int duration = boardLevelData.Duration;
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            _targetTime = DateTime.Now.AddSeconds(duration);
            _previousRemainingSeconds = duration;
            TickTimer().Forget();
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
                    continue;
                }

                _previousRemainingSeconds = currentRemainingTotalSeconds;
                
                if (currentRemainingTotalSeconds >= 0)
                {
                    EventBus.UpdateTimer(currentRemainingTotalSeconds);
                    continue;
                }
                
                EventBus.TimeIsUp();
                break;
            }
        }
    }
}