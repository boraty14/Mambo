using PrimeTween;
using UnityEngine;

namespace Game.Scripts.Tile
{
    [CreateAssetMenu]
    public class TileSettings : ScriptableObject
    {
        public float MoveDuration;
        public Ease MoveEase;
    }
}
