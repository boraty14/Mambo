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
            EventBus.OnUpdateTimer += OnUpdateTimer;
        }

        private void OnDisable()
        {
            EventBus.OnUpdateTimer -= OnUpdateTimer;
        }

        private void OnUpdateTimer(int duration)
        {
            _timeText.text = duration.ToString();
        }
    }
}