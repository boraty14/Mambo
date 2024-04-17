using TMPro;
using UnityEngine;

namespace Game.Scripts.Timer
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private GameObject _gameObject;
        private ITimer _timer;

        private void Start()
        {
            _timer = Provider.ServiceLocator.GetService<ITimer>();
            RegisterEvents();
        }
        
        private void OnEnable()
        {
            RegisterEvents();
        }
        
        private void OnDisable()
        {
            UnregisterEvents();
        }

        public void Toggle(bool state)
        {
            _gameObject.SetActive(state);
        }

        private void RegisterEvents()
        {
            if (_timer == null)
            {
                return;
            }
            
            _timer.OnTimerUpdate -= OnTimerUpdate;
            _timer.OnTimeIsUp -= OnTimeIsUp;
            _timer.OnTimerUpdate += OnTimerUpdate;
            _timer.OnTimeIsUp += OnTimeIsUp;
        }
        
        private void UnregisterEvents()
        {
            if (_timer == null)
            {
                return;
            }
            
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