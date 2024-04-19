using Cysharp.Threading.Tasks;
using Game.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Board
{
    public class BoardInputHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField] private BoardEntity _boardEntity;
        [SerializeField] private MoveProcessor _moveProcessor;
        [SerializeField] private Transform _boardParent;

        public const int UnselectedIndex = -1;
        
        private Camera _camera;
        private int _selectedPieceIndex = UnselectedIndex;
        private bool _isProcessingMove;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isProcessingMove)
            {
                return;
            }

            int oldSelectedIndex = _selectedPieceIndex;
            var boardPosition = GetBoardPosition(eventData);
            int tileIndex = _boardEntity.GetTileIndex(boardPosition);
            _selectedPieceIndex = tileIndex;

            if (_selectedPieceIndex == oldSelectedIndex)
            {
                _moveProcessor.ToggleSelect(oldSelectedIndex,false);
                return;
            }
            
            if (_selectedPieceIndex == UnselectedIndex)
            {
                if (oldSelectedIndex == UnselectedIndex)
                {
                    return;
                }
                
                _moveProcessor.ToggleSelect(oldSelectedIndex,false);
                return;
            }
            
            _moveProcessor.ToggleSelect(_selectedPieceIndex,true);

            if (oldSelectedIndex != UnselectedIndex)
            {
                SwapSelectedPieces(oldSelectedIndex,_selectedPieceIndex).Forget();
            }
        }
        

        public void OnDrag(PointerEventData eventData)
        {
            if (_isProcessingMove)
            {
                return;
            }
            
            var boardPosition = GetBoardPosition(eventData);
            int tileIndex = _boardEntity.GetTileIndex(boardPosition);
            if (tileIndex != UnselectedIndex && tileIndex != _selectedPieceIndex && _selectedPieceIndex != UnselectedIndex)
            {
                _moveProcessor.ToggleSelect(tileIndex,true);
                SwapSelectedPieces(tileIndex,_selectedPieceIndex).Forget();
            }
            
        }

        private async UniTaskVoid SwapSelectedPieces(int firstPieceIndex, int secondPieceIndex)
        {
            _isProcessingMove = true;
            await _moveProcessor.ProcessMove(firstPieceIndex, secondPieceIndex);
            _selectedPieceIndex = UnselectedIndex;
            _isProcessingMove = false;
        }

        private Vector3 GetBoardPosition(PointerEventData eventData)
        {
            var position = eventData.position;
            var worldPosition = _camera.ScreenToWorldPoint(new Vector3(position.x, position.y, 0f));
            return _boardParent.InverseTransformPoint(worldPosition);
        }
    }
}
