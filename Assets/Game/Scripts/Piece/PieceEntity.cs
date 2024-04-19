using System.Collections.Generic;
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
        [SerializeField] private BlastEffectEntity _blastEffect;
        
        private const float MinMoveDistance = 0.01f;

        public EPiece PieceType { get; private set; }
        private Dictionary<EPiece, PieceProperties> _pieceProperties;

        public void Initialize(Dictionary<EPiece, PieceProperties> pieceProperties)
        {
            _pieceProperties = pieceProperties;
        }

        public void SetPiece(EPiece pieceType)
        {
            PieceType = pieceType;
            _spriteRenderer.sprite = _pieceProperties[pieceType].Sprite;
            ToggleSelect(false);
        }

        public void ToggleSelect(bool state)
        {
            _transform.localScale = state ? _pieceSettings.SelectScale : _pieceSettings.NormalScale;
            var color = _spriteRenderer.color;
            color.a = state ? _pieceSettings.SelectAlpha : _pieceSettings.UnselectAlpha;
            _spriteRenderer.color = color;
        }

        public void SetToPosition(Vector3 target)
        {
            _transform.localPosition = target;
        }

        public async UniTask Blast()
        {
            _blastEffect.SetEffect(PieceType);
            await Tween.Scale(_transform, _pieceSettings.BlastScale, _pieceSettings.BlastDuration,
                _pieceSettings.BlastEase);
            //TODO add particle pool? latest thing!
        }

        public async UniTask MoveToPosition(Vector3 target)
        {
            var distance = Vector3.Distance(_transform.localPosition, target);
            if (distance <= MinMoveDistance)
            {
                return;
            }
            await Tween.LocalPosition(_transform, target, _pieceSettings.MoveDuration * distance,
                _pieceSettings.MoveEase);
        }

        public Vector3 GetPosition()
        {
            return _transform.position;
        }
    }
}