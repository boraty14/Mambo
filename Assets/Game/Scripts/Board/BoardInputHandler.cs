using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Board
{
    public class BoardInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private Transform _boardParent;
        private bool _isInputEnabled;
        public event Action<PointerEventData> OnPointerDownEvent;
        public event Action<PointerEventData> OnPointerUpEvent;
        public event Action<PointerEventData> OnDragEvent;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log(GetBoardPosition(eventData));
            OnPointerDownEvent?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(eventData);
        }

        private Vector3 GetBoardPosition(PointerEventData eventData)
        {
            var position = eventData.position;
            var worldPosition = _camera.ScreenToWorldPoint(new Vector3(position.x, position.y, 0f));
            return _boardParent.InverseTransformPoint(worldPosition);
        }
    }
}
