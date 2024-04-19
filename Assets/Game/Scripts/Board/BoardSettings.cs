using UnityEngine;

namespace Game.Scripts.Board
{
    [CreateAssetMenu]
    public class BoardSettings : ScriptableObject
    {
        
        public float BoardLengthFactor;
        public float BoardEdgeOffset;
        [Range(0f, 1f)] public float TileInteractionPercentage;
        [Range(0f, 1f)] public float TileSize;

    }
}
