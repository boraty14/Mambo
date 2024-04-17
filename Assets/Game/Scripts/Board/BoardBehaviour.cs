using Game.Scripts.Tile;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Scripts.Board
{
    public class BoardBehaviour : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private BoardInputHandler _boardInputHandler;
        [SerializeField] private Transform _tileBackgroundPrefab;
        private TileBehaviour[] _tiles;
        private NativeArray<ETile> _boardData;
        private NativeList<int2> _matchPositions;
        private int _tileCount;

        private void Start()
        {
            _matchPositions = new NativeList<int2>(Allocator.Persistent);
            _tileCount = _boardSettings.Height * _boardSettings.Width;
            _tiles = new TileBehaviour[_tileCount];
            _boardData = new NativeArray<ETile>(_tileCount, Allocator.Persistent);
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            
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
                _boardData[i] = _tiles[i].TileType;
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