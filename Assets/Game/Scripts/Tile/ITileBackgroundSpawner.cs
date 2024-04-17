using Game.Scripts.Binding;

namespace Game.Scripts.Tile
{
    public interface ITileBackgroundSpawner : IService
    {
        TileBackground GetTileBackground();
        void ReleaseTileBackground(TileBackground tileBackground);
    }
}