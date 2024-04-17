using Game.Scripts.Piece;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Scripts.Board
{
    public class BoardGenerator : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private TileBackgroundSpawner _tileBackgroundSpawner;
        [SerializeField] private SpriteRenderer _backgroundSprite;
        [SerializeField] private BoxCollider2D _boardCollider;
        
        private TileBackgroundEntity[] _tileBackgrounds;
        private int _tileCount;
        private float _boardStartX;
        private float _boardStartY;
        
        private NativeArray<EPiece> _boardData;
        private NativeList<int2> _matchPositions;
        

        private void Awake()
        {
            _tileCount = _boardSettings.Height * _boardSettings.Width;
            _tileBackgrounds = new TileBackgroundEntity[_tileCount];
            
            var boardLengthFactor = _boardSettings.BoardLengthFactor;
            _boardStartX = -(_boardSettings.Width - 1) * 0.5f * boardLengthFactor;
            _boardStartY = -(_boardSettings.Height - 1) * 0.5f * boardLengthFactor;
            
            var sizeX = boardLengthFactor * _boardSettings.Width + _boardSettings.BoardEdgeOffset;
            var sizeY = boardLengthFactor * _boardSettings.Height + _boardSettings.BoardEdgeOffset;
            var sizeVector = new Vector2(sizeX, sizeY);

            _backgroundSprite.size = sizeVector;
            _boardCollider.size = sizeVector;
        }

        private void OnEnable()
        {
            SignalBus.StartGame += OnStartGame;
            SignalBus.EndGame += OnEndGame;

        }

        private void OnDisable()
        {
            SignalBus.StartGame -= OnStartGame;
            SignalBus.EndGame -= OnEndGame;
        }
        
        private void OnStartGame()
        {
            _boardCollider.enabled = true;
            _backgroundSprite.enabled = true;
            
            Vector3 scale = Vector3.one * _boardSettings.TileSize;
                
            for (int i = 0; i < _tileCount; i++)
            {
                var tileBackground =  _tileBackgroundSpawner.GetTileBackground();
                var widthIndex = i % _boardSettings.Width;
                var heightIndex = i / _boardSettings.Width;
                tileBackground.SetTransform(GetTilePosition(widthIndex,heightIndex), scale);
                _tileBackgrounds[i] = tileBackground;
            }
        }

        private Vector3 GetTilePosition(int widthIndex, int heightIndex)
        {
            var positionX = _boardStartX + (widthIndex * _boardSettings.BoardLengthFactor);
            var positionY = _boardStartY + (heightIndex * _boardSettings.BoardLengthFactor);
            return new Vector3(positionX, positionY, 0f);

        }
        
        private void OnEndGame()
        {
            _boardCollider.enabled = false;
            _backgroundSprite.enabled = false;
            
            for (int i = 0; i < _tileCount; i++)
            {
                _tileBackgroundSpawner.ReleaseTileBackground(_tileBackgrounds[i]);
            }
        }

        private void Start()
        {
            _matchPositions = new NativeList<int2>(Allocator.Persistent);
            _boardData = new NativeArray<EPiece>(_tileCount, Allocator.Persistent);
        }

        private void OnDestroy()
        {
            _matchPositions.Dispose();
            _boardData.Dispose();
        }

        private void DetectMatches()
        {
            _matchPositions.Clear();

            for (int i = 0; i < _tileCount; i++)
            {
                //_boardData[i] = _pieces[i].TileType;
            }

            MatchDetectionJob matchJob = new MatchDetectionJob
            {
                Board = _boardData,
                BoardWidth = _boardSettings.Width,
                BoardHeight = _boardSettings.Height,
                MatchPositions = _matchPositions
            };

            JobHandle jobHandle = matchJob.Schedule();

            jobHandle.Complete();

            // Now matchPositions contains the positions of matched tiles
            // Do whatever you need to do with this information
        }
    }
}