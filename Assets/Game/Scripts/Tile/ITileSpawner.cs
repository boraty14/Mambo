using Game.Scripts.Binding;

namespace Game.Scripts.Tile
{
    public interface ITileSpawner : IService
    {
        TileBehaviour GetTile(ETile tileType);
        void ReleaseTile(TileBehaviour tile);
    }
}