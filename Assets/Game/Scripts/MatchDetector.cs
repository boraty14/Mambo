using Game.Scripts.Tile;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class MatchDetector : MonoBehaviour
{
    [SerializeField] private BoardSettings _boardSettings;
    private ETile[] _tiles;
    private NativeArray<ETile> _boardData;
    private NativeList<int2> _matchPositions;
    private int _tileCount;

    private void Start()
    {
        _matchPositions = new NativeList<int2>(Allocator.Persistent);
        _tileCount = _boardSettings.Height * _boardSettings.Width;
        _tiles = new ETile[_tileCount];
        _boardData = new NativeArray<ETile>(_tileCount, Allocator.Persistent);
        DetectMatches();
    }

    private void OnDestroy()
    {
        _matchPositions.Dispose();
    }

    private void DetectMatches()
    {
        _matchPositions.Clear();

        for (int i = 0; i < _tileCount; i++)
        {
            _boardData[i] = _tiles[i];
        }

        MatchDetectionJob matchJob = new MatchDetectionJob
        {
            board = _boardData,
            boardWidth = _boardSettings.Width,
            boardHeight = _boardSettings.Height,
            matchPositions = _matchPositions
        };

        JobHandle jobHandle = matchJob.Schedule();

        jobHandle.Complete();

        // Now matchPositions contains the positions of matched tiles
        // Do whatever you need to do with this information
    }
}