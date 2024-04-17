using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

namespace Game.Scripts.Piece
{
    public class PieceEntity : MonoBehaviour
    {
        [SerializeField] private PieceSettings _tileSettings;
        [SerializeField] private Transform _transform;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public EPiece TileType { get; private set; }

        public void SetTile(EPiece tileType, Sprite sprite)
        {
            TileType = tileType;
            _spriteRenderer.sprite = sprite;
        }

        public void SetToPosition(Vector3 target)
        {
            _transform.position = target;
        }

        public async UniTask MoveToPosition(Vector3 target)
        {
            var distance = Vector3.Distance(_transform.position, target);
            await Tween.Position(_transform, target, _tileSettings.MoveDuration * distance, _tileSettings.MoveEase);
        }
    }
}