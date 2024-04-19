using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Board;
using Game.Scripts.Piece;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.GamePlay
{
    public class MoveProcessor : MonoBehaviour
    {
        [SerializeField] private BoardEntity _boardEntity;
        [SerializeField] private PieceSpawner _pieceSpawner;

        private PieceEntity[] _pieces;
        private BoardLevelData _boardLevelData;

        private NativeArray<EPiece> _piecesData;
        private NativeArray<bool> _matchBoardData;

        private readonly List<UniTask> _blastPieceTasks = new();
        private readonly List<UniTask> _movePiecesTasks = new();
        private readonly List<UniTask> _swapPiecesTasks = new();
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

            DisposeArrays();
            _piecesData = new NativeArray<EPiece>(_boardLevelData.TileCount, Allocator.Persistent);
            _matchBoardData = new NativeArray<bool>(_boardLevelData.TileCount, Allocator.Persistent);

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

        public void ToggleSelect(int index, bool state)
        {
            if (index < 0 || index >= _boardLevelData.TileCount || _pieces[index] == null)
            {
                return;
            }

            _pieces[index].ToggleSelect(state);
        }

        public async UniTask ProcessMove(int firstPieceIndex, int secondPieceIndex)
        {
            await SwapPieces(firstPieceIndex, secondPieceIndex);
            CheckMatches();
            if (!IsMatch())
            {
                await SwapPieces(firstPieceIndex, secondPieceIndex);
                ToggleSelect(firstPieceIndex,false);
                ToggleSelect(secondPieceIndex,false);
                return;
            }

            await ProcessBlast();

            CheckMatches();
            while (IsMatch())
            {
                await ProcessBlast();
                CheckMatches();
            }
        }
        
        private async UniTask SwapPieces(int firstPieceIndex, int secondPieceIndex)
        {
            Debug.Log($"{firstPieceIndex} {secondPieceIndex}");
            (_pieces[firstPieceIndex], _pieces[secondPieceIndex]) =
                (_pieces[secondPieceIndex], _pieces[firstPieceIndex]);
            
            _swapPiecesTasks.Add(_pieces[firstPieceIndex]
                .MoveToPosition(_boardEntity.GetTilePosition(firstPieceIndex)));
            _swapPiecesTasks.Add(_pieces[secondPieceIndex]
                .MoveToPosition(_boardEntity.GetTilePosition(secondPieceIndex)));

            await UniTask.WhenAll(_swapPiecesTasks);
            _swapPiecesTasks.Clear();
        }

        private async UniTask ProcessBlast()
        {
            await BlastPieces();
            SetNewIndices();
            GenerateNewPieces();
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            await MovePiecesToNewPositions();
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
                    _blastPieceTasks.Add(BlastPiece(i));
                }
            }

            await UniTask.WhenAll(_blastPieceTasks);
            _blastPieceTasks.Clear();
        }

        private async UniTask BlastPiece(int index)
        {
            await _pieces[index].Blast();
            _pieceSpawner.ReleasePiece(_pieces[index]);
            _pieces[index] = null;
        }

        private void SetNewIndices()
        {
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                if (_pieces[i] == null)
                {
                    continue;
                }
                
                int fallAmount = 0;
                for (int j = i - _boardLevelData.Width; j >= 0; j -= _boardLevelData.Width)
                {
                    if (_matchBoardData[j])
                    {
                        fallAmount++;
                    }                    
                }

                _pieces[i - fallAmount * _boardLevelData.Width] = _pieces[i];
            }
        }

        private void GenerateNewPieces()
        {
            for (int i = 0; i < _boardLevelData.Width; i++)
            {
                int generateAmount = 0;
                
                for (int j = 0; j < _boardLevelData.Height; j++)
                {
                    if (_matchBoardData[i + j * _boardLevelData.Width])
                    {
                        generateAmount++;
                    }
                }

                for (int j = 0; j < generateAmount; j++)
                {
                    var piece = _pieceSpawner.GetRandomPiece();
                    int spawnIndex = i + _boardLevelData.TileCount + _boardLevelData.Width * j;
                    piece.SetToPosition(_boardEntity.GetTilePosition(spawnIndex));
                    _pieces[spawnIndex - generateAmount * _boardLevelData.Width] = piece;
                }
            }
        }

        private async UniTask MovePiecesToNewPositions()
        {
            for (int i = 0; i < _boardLevelData.TileCount; i++)
            {
                _movePiecesTasks.Add(_pieces[i].MoveToPosition(_boardEntity.GetTilePosition(i)));
            }

            await UniTask.WhenAll(_movePiecesTasks);
            _movePiecesTasks.Clear();
        }

        private void DisposeArrays()
        {
            _piecesData.Dispose();
            _matchBoardData.Dispose();
        }

        private void OnDestroy()
        {
            DisposeArrays();
        }
    }
}