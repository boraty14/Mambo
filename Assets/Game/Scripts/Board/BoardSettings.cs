using UnityEngine;

namespace Game.Scripts.Board
{
    [CreateAssetMenu]
    public class BoardSettings : ScriptableObject
    {
        [Range(1, 15)] public int Height;
        [Range(1, 9)] public int Width;
        [Range(0f, 1f)] public float TileInteractionPercentage;
        [Range(0f, 5f)] public float TileSize;
    }
}
