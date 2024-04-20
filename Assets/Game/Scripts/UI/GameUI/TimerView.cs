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
            _timer.OnTimerUpdate += OnTimerUpdate;
            _timer.OnTimeIsUp += OnTimeIsUp;
        }

        private void OnDisable()
        {
            _timer.OnTimerUpdate -= OnTimerUpdate;
            _timer.OnTimeIsUp -= OnTimeIsUp;
        }

        private void OnTimerUpdate(int duration)
        {
            _timeText.text = duration.ToString();
        }

        private void OnTimeIsUp()
        {
            _timeText.text = "0";
        }
    }
}