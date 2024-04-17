using System;
using PrimeTween;
using UnityEngine;

namespace Game.Scripts.Tile
{
    [CreateAssetMenu]
    public class TileSettings : ScriptableObject
    {
        public float MoveDuration;
        public Ease MoveEase;
        public TileProperties[] TilesProperties;
    }

    [Serializable]
    public class TileProperties
    {
        public ETile Type;
        public Sprite Sprite;
    }
}
