using Game.Scripts.Binding;

namespace Game.Scripts.Input
{
    public interface IInputHandler : IService
    {
        void ToggleInput(bool state);
        
        
    }
}