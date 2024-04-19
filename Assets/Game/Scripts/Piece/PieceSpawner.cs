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
            foreach (var pieceProperties in _pieceSettings.PiecesProperties)
            {
                _piecesProperties.TryAdd(pieceProperties.Type, pieceProperties);
            }

            _pieceTypeCount = _pieceSettings.PiecesProperties.Length;
            InitPool(_piecePrefab);
        }

        protected override PieceEntity CreateSetup()
        {
            var piece = base.CreateSetup();
            piece.Initialize(_piecesProperties);
            return piece;
        }

        public PieceEntity GetRandomPiece()
        {
            int randomIndex = Random.Range(0, _pieceTypeCount);
            return GetPiece((EPiece)randomIndex);
        }

        public PieceEntity GetPiece(EPiece pieceType)
        {
            var piece = Get();
            piece.SetPiece(pieceType);
            return piece;
        }

        public void ReleasePiece(PieceEntity tile)
        {
            Release(tile);
        }
    }
}