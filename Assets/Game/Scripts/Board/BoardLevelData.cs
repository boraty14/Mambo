using UnityEngine;

namespace Game.Scripts.Board
{
    [CreateAssetMenu]
    public class BoardLevelData : ScriptableObject
    {
        [Range(1, 10)] public int Height;
        [Range(1, 10)] public int Width;
        
        public int TileCount => Height * Width;
        public int Duration;
    }
}