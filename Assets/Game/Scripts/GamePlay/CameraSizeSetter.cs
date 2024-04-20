using Game.Scripts.Board;
using UnityEngine;

namespace Game.Scripts.GamePlay
{
    public class CameraSizeSetter : MonoBehaviour
    {
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private BoardSettings _boardSettings;

        private void OnEnable()
        {
            EventBus.OnSetBoardLevelData += OnSetBoardLevelData;
        }

        private void OnDisable()
        {
            EventBus.OnSetBoardLevelData -= OnSetBoardLevelData;
        }

        private void OnSetBoardLevelData(BoardLevelData boardLevelData)
        {
            var widthSize = boardLevelData.Width;
            var heightSize = boardLevelData.Height * _gameCamera.aspect;

            var aspect = widthSize > heightSize ? _gameCamera.aspect : 1f;
            
            var originalSize = (_boardSettings.BoardLengthFactor * boardLevelData.Width) /
                               (2f * aspect);

            var offsetSize = (1f + _boardSettings.BoardEdgeOffset) * originalSize;

            _gameCamera.orthographicSize = offsetSize;
        }
    }
}
