using UnityEngine;

namespace Game.Scripts.GamePlay
{
    public class PieceSelector : MonoBehaviour
    {
        [SerializeField] private MoveProcessor _moveProcessor;

        private bool _isProcessingMove;
        private int _currentSelectedPieceIndex = -1;
        
        private void OnEnable()
        {
            EventBus.OnSelectPiece += OnSelectPiece;
        }

        private void OnDisable()
        {
            EventBus.OnSelectPiece -= OnSelectPiece;
        }

        private void OnSelectPiece(int pieceIndex)
        {
            if (_currentSelectedPieceIndex != -1)
            {
                _moveProcessor.ToggleSelect(_currentSelectedPieceIndex,false);
            }

            _currentSelectedPieceIndex = pieceIndex;
            _moveProcessor.ToggleSelect(_currentSelectedPieceIndex,true);
        }
        
        
    }
}