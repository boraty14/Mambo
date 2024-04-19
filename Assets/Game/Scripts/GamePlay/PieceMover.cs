using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Board;
using Game.Scripts.Piece;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Game.Scripts.GamePlay
{
    public class PieceMover : MonoBehaviour
    {
        [SerializeField] private PieceSpawner _pieceSpawner;
        private NativeArray<EPiece> _piecesData;
        private NativeArray<bool> _matchPositions;
        private PieceEntity[] _pieces;
        private BoardLevelData _boardLevelData;

        private readonly List<UniTask> _blastPieceTasks = new();

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
            _piecesData = new NativeArray<EPiece>(_boardLevelData.TileCount, Allocator.Persistent);
            _matchPositions = new NativeArray<bool>(_boardLevelData.TileCount, Allocator.Persistent);
            _pieces = new PieceEntity[_boardLevelData.TileCount];
        }
        
        private void OnEndGame()
        {
            foreach (var piece in _pieces)
            {
                if (piece != null)
                {
                    _pieceSpawner.ReleasePiece(piece);
                }
            }
        }
        
        private void DetectMatches()
        {
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                _matchPositions[i] = false;
                _piecesData[i] = _pieces[i].PieceType;
            }

            MatchDetectionJob matchJob = new MatchDetectionJob
            {
                Board = _piecesData,
                BoardWidth = _boardLevelData.Width,
                BoardHeight = _boardLevelData.Height,
                MatchPositions = _matchPositions
            };

            JobHandle jobHandle = matchJob.Schedule();

            jobHandle.Complete();

            // check if there is a match
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                if (_matchPositions[i] == true)
                {
                    BlastPieces().Forget();
                    return;
                }
            }
            
            ReverseLastMove();
        }

        private async UniTask BlastPieces()
        {
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                if (_matchPositions[i] == true)
                {
                    _blastPieceTasks.Add(BlastPiece(_pieces[i]));                    
                }
            }

            await UniTask.WhenAll(_blastPieceTasks);
            _blastPieceTasks.Clear();
        }

        private async UniTask BlastPiece(PieceEntity piece)
        {
            await piece.Blast();
            _pieceSpawner.ReleasePiece(piece);
        }

        private void ReverseLastMove()
        {
            
        }

        private void OnDestroy()
        {
            _piecesData.Dispose();
            _matchPositions.Dispose();
        }
    }
}