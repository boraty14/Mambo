using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

namespace Game.Scripts.Piece
{
    public class PieceEntity : MonoBehaviour
    {
        [SerializeField] private PieceSettings _pieceSettings;
        [SerializeField] private Transform _transform;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public EPiece PieceType { get; private set; }

        public void SetPiece(EPiece tileType, Sprite sprite)
        {
            PieceType = tileType;
            _spriteRenderer.sprite = sprite;
            _transform.localScale = Vector3.one;
        }

        public void ToggleSelect(bool state)
        {
            var color = _spriteRenderer.color;
            color.a = state ? _pieceSettings.SelectAlpha : _pieceSettings.UnselectAlpha;
            _spriteRenderer.color = color;
        }

        public void SetToPosition(Vector3 target)
        {
            _transform.position = target;
        }

        public async UniTask Blast()
        {
            await Tween.Scale(_transform, Vector3.zero, _pieceSettings.BlastDuration, _pieceSettings.BlastEase);
            //TODO add particle pool? latest thing!
        }

        public async UniTask MoveToPosition(Vector3 target)
        {
            var distance = Vector3.Distance(_transform.position, target);
            await Tween.Position(_transform, target, _pieceSettings.MoveDuration * distance, _pieceSettings.MoveEase);
        }
    }
}