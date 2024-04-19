using System;
using PrimeTween;
using UnityEngine;

namespace Game.Scripts.Piece
{
    [CreateAssetMenu]
    public class PieceSettings : ScriptableObject
    {
        public float MoveDuration;
        public Ease MoveEase;
        public float BlastDuration;
        public Ease BlastEase;
        [Range(0f, 1f)] public float SelectAlpha;
        [Range(0f, 1f)] public float UnselectAlpha;
        public PieceProperties[] PiecesProperties;
    }

    [Serializable]
    public class PieceProperties
    {
        public EPiece Type;
        public Sprite Sprite;
    }
}
