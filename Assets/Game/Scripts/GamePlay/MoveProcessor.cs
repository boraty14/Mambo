using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Board;
using Game.Scripts.Piece;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

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
        private NativeArray<int> _newBoardIndices;
        private int[] _columnBlastAmount;
        private bool _isPieceMoving;

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
            _newBoardIndices = new NativeArray<int>(_boardLevelData.TileCount, Allocator.Persistent);
            _columnBlastAmount = new int[_boardLevelData.Width];

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
            if (_isPieceMoving)
            {
                return;
            }

            _isPieceMoving = true;

            await SwapPieces(firstPieceIndex, secondPieceIndex);
            CheckMatches();
            if (!IsMatch())
            {
                await SwapPieces(firstPieceIndex, secondPieceIndex);
                _isPieceMoving = false;
                return;
            }

            await ProcessBlast();

            CheckMatches();
            while (IsMatch())
            {
                await ProcessBlast();
                CheckMatches();
            }

            _isPieceMoving = false;
        }
        
        private async UniTask SwapPieces(int firstPieceIndex, int secondPieceIndex)
        {
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

            for (int i = 0; i < _boardLevelData.Width; i++)
            {
                _columnBlastAmount[i] = 0;
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
                    _columnBlastAmount[i % _boardLevelData.Width]++;
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
                int newIndex = _newBoardIndices[i];
                if (newIndex == i || _pieces[i] != null)
                {
                    continue;
                }

                _pieces[i] = _pieces[newIndex];
                _pieces[newIndex] = null;
            }
        }

        private void GenerateNewPieces()
        {
            for (int i = _boardLevelData.TileCount - 1; i >= 0; i--)
            {
                if (_pieces[i] != null)
                {
                    continue;
                }

                var piece = _pieceSpawner.GetRandomPiece();
                int columnIndex = i % _boardLevelData.Width;
                piece.SetToPosition(
                    _boardEntity.GetTilePosition(i + _boardLevelData.Width * _columnBlastAmount[columnIndex]));
                _columnBlastAmount[columnIndex] -= 1;
                _pieces[i] = piece;
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
            _newBoardIndices.Dispose();
        }

        private void OnDestroy()
        {
            DisposeArrays();
        }
    }
}