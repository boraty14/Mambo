using Game.Scripts.Piece;
using UnityEngine;

namespace Game.Scripts.GamePlay
{
    public class PieceSelector : MonoBehaviour
    {
        private PieceEntity _lastSelectedPiece;
        private PieceEntity _currentSelectedPiece;
        
        private void OnEnable()
        {
            EventBus.OnSelectPiece += OnSelectPiece;
        }

        private void OnDisable()
        {
            EventBus.OnSelectPiece -= OnSelectPiece;
        }

        private void OnSelectPiece(PieceEntity piece)
        {
            if (_lastSelectedPiece != null)
            {
                _lastSelectedPiece.ToggleSelect(false);
            }

            _currentSelectedPiece = piece;
            _currentSelectedPiece.ToggleSelect(true);
        }
        
        
    }
}