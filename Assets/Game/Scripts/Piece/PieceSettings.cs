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
        public PieceProperties[] PiecesProperties;
    }

    [Serializable]
    public class PieceProperties
    {
        public EPiece Type;
        public Sprite Sprite;
    }
}
