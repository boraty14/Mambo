using Game.Scripts.Binding;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Tile
{
    public class TileSpawner : PoolerBase<Tile>, ITileSpawner
    {
        [SerializeField] private Tile _tilePrefab;
        public IServiceLocator ServiceLocator { get; set; }
    
        private void Start()
        {
            InitPool(_tilePrefab);
        }
    
        public Tile GetTile(ETile tileType)
        {
            var tile = Get();
            // TODO set tile according to type

            return tile;
        }

        public void ReleaseTile(Tile tile)
        {
            Release(tile);
        }
    }
}
