using Game.Scripts.Binding;

namespace Game.Scripts.Tile
{
    public interface ITileSpawner : IService
    {
        Tile GetTile(ETile tileType);

        void ReleaseTile(Tile tile);
    }
}