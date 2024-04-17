using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Board
{
    public class BoardInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private Transform _boardParent;
        
        private Camera _camera;
        private int _selectedPieceIndex = -1;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var boardPosition = GetBoardPosition(eventData);
            
            Debug.Log(boardPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_selectedPieceIndex == -1)
            {
                return;
            }
        }

        private Vector3 GetBoardPosition(PointerEventData eventData)
        {
            var position = eventData.position;
            var worldPosition = _camera.ScreenToWorldPoint(new Vector3(position.x, position.y, 0f));
            return _boardParent.InverseTransformPoint(worldPosition);
        }
    }
}
