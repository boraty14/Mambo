using System.Collections.Generic;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Tile
{
    public class TileSpawner : PoolerBase<TileBehaviour>, ITileSpawner
    {
        [SerializeField] private TileBehaviour _tilePrefab;
        [SerializeField] private TileSettings _tileSettings;

        private readonly Dictionary<ETile, TileProperties> _tilesProperties;

        private void Awake()
        {
            InitPool(_tilePrefab);
            foreach (var tileProperties in _tileSettings.TilesProperties)
            {
                _tilesProperties.TryAdd(tileProperties.Type,tileProperties);
            }
            Provider.ServiceLocator.AddService<ITileSpawner,TileSpawner>(this);
        }
    
        public TileBehaviour GetTile(ETile tileType)
        {
            var tile = Get();
            tile.SetTile(tileType, _tilesProperties[tileType].Sprite);
            return tile;
        }

        public void ReleaseTile(TileBehaviour tile)
        {
            Release(tile);
        }
    }
}
