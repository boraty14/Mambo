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
        [SerializeField] private BoardEntity _boardEntity;
        [SerializeField] private PieceSpawner _pieceSpawner;

        private PieceEntity[] _pieces;
        private BoardLevelData _boardLevelData;

        private NativeArray<EPiece> _piecesData;
        private NativeArray<bool> _matchBoardData;
        private NativeArray<int> _newBoardIndices;

        private readonly List<UniTask> _blastPieceTasks = new();
        
        private const int RandomSeed = 9999;


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
            _matchBoardData = new NativeArray<bool>(_boardLevelData.TileCount, Allocator.Persistent);
            _newBoardIndices = new NativeArray<int>(_boardLevelData.TileCount, Allocator.Persistent);
            _pieces = new PieceEntity[_boardLevelData.TileCount];
            InitializePieces();
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

        private void InitializePieces()
        {
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                _pieces[i] = _pieceSpawner.GetRandomPiece();
            }

            CheckMatches();
            while (IsMatch())
            {
                ShufflePieces();
                CheckMatches();
            }
            
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                _pieces[i].SetToPosition(_boardEntity.GetTilePosition(i));
            }
        }

        private void ProcessMove()
        {
            CheckMatches();
            if (IsMatch())
            {
                BlastPieces().Forget();
                return;
            }

            ReverseLastMove();
        }
        
        private void ReverseLastMove()
        {
        }
        
        private void ShufflePieces()
        {
            ResetArrays();

            ShuffleJob matchJob = new ShuffleJob()
            {
                Board = _piecesData,
                Seed = Random.Range(1, RandomSeed)
            };
            
            JobHandle jobHandle = matchJob.Schedule();
            jobHandle.Complete();

            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                _pieces[i].SetPiece(_piecesData[i]);
            }
        }

        private void CheckMatches()
        {
            ResetArrays();

            MatchDetectionJob matchJob = new MatchDetectionJob
            {
                Board = _piecesData,
                MatchBoard = _matchBoardData,
                NewBoardIndices = _newBoardIndices,
                BoardWidth = _boardLevelData.Width,
                BoardHeight = _boardLevelData.Height,
            };

            JobHandle jobHandle = matchJob.Schedule();
            jobHandle.Complete();
        }
        
        private void ResetArrays()
        {
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                _piecesData[i] = _pieces[i].PieceType;
                _matchBoardData[i] = false;
                _newBoardIndices[i] = i;
            }
        }

        private bool IsMatch()
        {
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                if (_matchBoardData[i])
                {
                    return true;
                }
            }

            return false;
        }

        private async UniTask BlastPieces()
        {
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                if (_matchBoardData[i])
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

        private void OnDestroy()
        {
            _piecesData.Dispose();
            _matchBoardData.Dispose();
            _newBoardIndices.Dispose();
        }
    }
}