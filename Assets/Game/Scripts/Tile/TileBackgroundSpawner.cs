using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Tile
{
    public class TileBackgroundSpawner : PoolerBase<TileBackground>, ITileBackgroundSpawner
    {
        [SerializeField] private TileBackground _tileBackgroundPrefab;
        
        private void Awake()
        {
            InitPool(_tileBackgroundPrefab);
            Provider.ServiceLocator.AddService<ITileBackgroundSpawner,TileBackgroundSpawner>(this);

        }

        
        public TileBackground GetTileBackground()
        {
            var tileBackground = Get();
            return tileBackground;
        }

        public void ReleaseTileBackground(TileBackground tileBackground)
        {
            Release(tileBackground);
        }
    }
}