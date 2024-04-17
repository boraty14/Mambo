using UnityEngine;

namespace Game.Scripts.Board
{
    [CreateAssetMenu]
    public class BoardSettings : ScriptableObject
    {
        [Range(1, 15)] public int Height;
        [Range(1, 10)] public int Width;
        public float BoardLengthFactor;
        public float BoardEdgeOffset;
        [Range(0f, 1f)] public float TileInteractionPercentage;
        [Range(0f, 1f)] public float TileSize;
    }
}
