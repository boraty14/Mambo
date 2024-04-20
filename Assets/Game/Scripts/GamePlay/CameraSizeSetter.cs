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

            var isWidthLonger = widthSize > heightSize;
            var aspect = isWidthLonger ? _gameCamera.aspect : 1f;
            var length = isWidthLonger ? boardLevelData.Width : boardLevelData.Height;
            
            var originalSize = (_boardSettings.BoardLengthFactor * length) / (2f * aspect);

            var offsetSize = (1f + _boardSettings.BoardEdgeOffset) * originalSize;

            _gameCamera.orthographicSize = offsetSize;
        }
    }
}
