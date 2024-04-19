using System.Collections.Generic;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Piece
{
    public class PieceSpawner : PoolerBase<PieceEntity>
    {
        [SerializeField] private PieceEntity _piecePrefab;
        [SerializeField] private PieceSettings _pieceSettings;

        private readonly Dictionary<EPiece, PieceProperties> _piecesProperties = new();
        private int _pieceTypeCount;

        private void Awake()
        {
            InitPool(_piecePrefab);
            foreach (var pieceProperties in _pieceSettings.PiecesProperties)
            {
                _piecesProperties.TryAdd(pieceProperties.Type, pieceProperties);
            }

            _pieceTypeCount = _pieceSettings.PiecesProperties.Length;
        }

        public PieceEntity GetRandomPiece()
        {
            int randomIndex = Random.Range(0, _pieceTypeCount);
            return GetPiece((EPiece)randomIndex);
        }

        public PieceEntity GetPiece(EPiece pieceType)
        {
            var piece = Get();
            piece.SetPiece(pieceType, _piecesProperties[pieceType].Sprite);
            return piece;
        }

        public void ReleasePiece(PieceEntity tile)
        {
            Release(tile);
        }
    }
}