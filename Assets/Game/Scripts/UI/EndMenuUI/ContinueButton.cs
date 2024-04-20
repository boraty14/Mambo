using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.EndMenuUI
{
    public class ContinueButton : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(OnContinueClick);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(OnContinueClick);
        }

        private void OnContinueClick()
        {
            EventBus.ReturnMainMenu();
        }
    }
}