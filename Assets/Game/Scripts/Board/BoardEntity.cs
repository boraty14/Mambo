using UnityEngine;

namespace Game.Scripts.Board
{
    public class BoardEntity : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private TileSpawner _tileBackgroundSpawner;
        [SerializeField] private SpriteRenderer _backgroundSprite;
        [SerializeField] private BoxCollider2D _boardCollider;
        private TileEntity[] _tiles;
        private Vector2 _boardStartPosition;
        private BoardLevelData _boardLevelData;

        private void OnEnable()
        {
            EventBus.OnSetBoardLevelData += OnSetBoardLevelData;
            EventBus.OnEndGame += OnEndGame;
        }

        private void OnDisable()
        {
            EventBus.OnSetBoardLevelData -= OnSetBoardLevelData;
            EventBus.OnEndGame -= OnEndGame;
        }

        private void OnSetBoardLevelData(BoardLevelData boardLevelData)
        {
            _boardLevelData = boardLevelData;
            GenerateBoard();
        }

        private void OnEndGame()
        {
            ReleaseBoard();
        }

        private void GenerateBoard()
        {
            // enable board
            _boardCollider.enabled = true;
            _backgroundSprite.enabled = true;
         
            // set board size
            var boardLengthFactor = _boardSettings.BoardLengthFactor;
            var sizeX = boardLengthFactor * _boardLevelData.Width + _boardSettings.BoardEdgeOffset;
            var sizeY = boardLengthFactor * _boardLevelData.Height + _boardSettings.BoardEdgeOffset;
            var size = new Vector2(sizeX, sizeY);
            _boardCollider.size = size;
            _backgroundSprite.size = size;

            // set board start anchor position
            _boardStartPosition = new Vector2(-(_boardLevelData.Width - 1) * 0.5f * boardLengthFactor,
                -(_boardLevelData.Height - 1) * 0.5f * boardLengthFactor);
            
            // generate tiles
            _tiles = new TileEntity[_boardLevelData.TileCount];
            Vector3 scale = Vector3.one * _boardSettings.TileSize;
                
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                var tileBackground =  _tileBackgroundSpawner.GetTileBackground();
                var widthIndex = i % _boardLevelData.Width;
                var heightIndex = i / _boardLevelData.Width;
                tileBackground.SetTransform(GetTilePosition(widthIndex,heightIndex), scale);
                _tiles[i] = tileBackground;
            }
        }
        
        private Vector3 GetTilePosition(int widthIndex, int heightIndex)
        {
            var positionX = _boardStartPosition.x + (widthIndex * _boardSettings.BoardLengthFactor);
            var positionY = _boardStartPosition.y + (heightIndex * _boardSettings.BoardLengthFactor);
            return new Vector3(positionX, positionY, 0f);
        }

        private void ReleaseBoard()
        {
            _boardCollider.enabled = false;
            _backgroundSprite.enabled = false;
            
            foreach (var tile in _tiles)
            {
                _tileBackgroundSpawner.ReleaseTileBackground(tile);
            }
        }
    }
}