using Game.Scripts.Binding;
using UnityEngine;

namespace Game.Scripts.Input
{
    public class InputHandler : MonoBehaviour, IInputHandler
    {
        public IServiceLocator ServiceLocator { get; set; }

        private bool _isInputEnabled;
    
        public void ToggleInput(bool state)
        {
            _isInputEnabled = state;
        }
    }
}
