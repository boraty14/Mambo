using Game.Scripts.Binding;
using Game.Scripts.Input;
using Game.Scripts.Tile;
using UnityEngine;

namespace Game.Scripts.Contexts
{
    public class GameContext : ContextBase
    {
        [SerializeField] private TileSpawner _tileSpawnerPrefab;
        [SerializeField] private InputHandler _inputHandlerPrefab;
        
        protected override void InstallBindings()
        {
            BindPrefab<IInputHandler,InputHandler>(_inputHandlerPrefab);
            BindPrefab<ITileSpawner,TileSpawner>(_tileSpawnerPrefab);
        }
    }
}