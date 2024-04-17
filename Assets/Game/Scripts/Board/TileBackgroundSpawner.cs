using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Board
{
    public class TileBackgroundSpawner : PoolerBase<TileBackgroundEntity>
    {
        [SerializeField] private TileBackgroundEntity _tileBackgroundPrefab;
        
        private void Awake()
        {
            InitPool(_tileBackgroundPrefab);
        }
        
        public TileBackgroundEntity GetTileBackground()
        {
            var tileBackground = Get();
            return tileBackground;
        }

        public void ReleaseTileBackground(TileBackgroundEntity tileBackground)
        {
            Release(tileBackground);
        }
    }
}