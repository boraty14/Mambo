using Game.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Board
{
    public class BoardInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private BoardEntity _boardEntity;
        [SerializeField] private MoveProcessor _moveProcessor;
        [SerializeField] private Transform _boardParent;

        private const int UnselectedIndex = -1;
        
        private Camera _camera;
        private int _selectedPieceIndex = UnselectedIndex;
        private bool _isProcessingMove;

        private void Start()
        {
            _camera = Camera.main;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isProcessingMove)
            {
                return;
            }

            bool hasAlreadySelectedPiece = _selectedPieceIndex != UnselectedIndex;
            var boardPosition = GetBoardPosition(eventData);
            int tileIndex = _boardEntity.GetTileIndex(boardPosition);
            EventBus.SelectPiece(tileIndex);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isProcessingMove)
            {
                return;
            }
            
            var boardPosition = GetBoardPosition(eventData);
            int tileIndex = _boardEntity.GetTileIndex(boardPosition);
            EventBus.SelectPiece(tileIndex);
            Debug.Log(tileIndex);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isProcessingMove)
            {
                return;
            }
            
            var boardPosition = GetBoardPosition(eventData);
            int tileIndex = _boardEntity.GetTileIndex(boardPosition);
            EventBus.SelectPiece(tileIndex);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isProcessingMove)
            {
                return;
            }
            
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
