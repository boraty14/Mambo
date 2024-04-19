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
        public Vector3 SelectScale;
        public Vector3 NormalScale;
        public Vector3 BlastScale;
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
