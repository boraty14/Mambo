using TMPro;
using UnityEngine;

namespace Game.Scripts.Timer
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private GameObject _gameObject;
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

        public void Toggle(bool state)
        {
            _gameObject.SetActive(state);
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