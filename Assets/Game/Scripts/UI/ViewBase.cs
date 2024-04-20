using UnityEngine;

namespace Game.Scripts.UI
{
    public abstract class ViewBase : MonoBehaviour
    {
        [SerializeField] private GameObject _view;

        protected void ToggleView(bool state)
        {
            _view.SetActive(state);
        }
    }
}