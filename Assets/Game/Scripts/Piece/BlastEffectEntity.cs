using UnityEngine;

namespace Game.Scripts.Piece
{
    public class BlastEffectEntity : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _transform;

        public void SetEffect(PieceEntity piece)
        {
            _transform.position = piece.GetPosition();
            
            switch (piece.PieceType)
            {
                case EPiece.Blue:
                    _animator.Play("BlueSplash", 0, 0f);
                    break;
                case EPiece.Green:
                    _animator.Play("GreenSplash", 0, 0f);
                    break;
                case EPiece.Orange:
                    _animator.Play("OrangeSplash", 0, 0f);
                    break;
                case EPiece.Red:
                    _animator.Play("RedSplash", 0, 0f);
                    break;
                case EPiece.Yellow:
                    _animator.Play("YellowSplash", 0, 0f);
                    break;
            }
        }
    }
}