using Game.Scripts.Board;
using Game.Scripts.GamePlay;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.GameUI
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private Timer _timer;

        private void OnEnable()
        {
            EventBus.OnSetBoardLevelData += OnSetBoardLevelData;
            EventBus.OnUpdateTimer += OnUpdateTimer;
        }

        private void OnDisable()
        {
            EventBus.OnSetBoardLevelData -= OnSetBoardLevelData;
            EventBus.OnUpdateTimer -= OnUpdateTimer;
        }

        private void OnSetBoardLevelData(BoardLevelData boardLevelData)
        {
            _timeText.text = boardLevelData.Duration.ToString();
        }

        private void OnUpdateTimer(int duration)
        {
            _timeText.text = duration.ToString();
        }
    }
}