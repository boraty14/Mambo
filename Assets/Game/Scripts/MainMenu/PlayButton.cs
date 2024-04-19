using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private GameObject _gameObject;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayClick);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayClick);
        }
        
        public void Toggle(bool state)
        {
            _gameObject.SetActive(state);
        }

        private void OnPlayClick()
        {
            EventBus.StartGame();
        }
    }
}