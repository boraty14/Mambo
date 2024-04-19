using Game.Scripts.Board;
using UnityEngine;

namespace Game.Scripts.GamePlay
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private BoardLevelData _boardLevelData;

        private void OnEnable()
        {
            EventBus.OnStartGame += OnStartGame;
        }

        private void OnDisable()
        {
            EventBus.OnStartGame -= OnStartGame;
        }

        private void OnStartGame()
        {
            EventBus.SetBoardLevelData(_boardLevelData);
        }
    }
}