using System.Collections.Generic;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Piece
{
    public class PieceSpawner : PoolerBase<PieceEntity>
    {
        [SerializeField] private PieceEntity _piecePrefab;
        [SerializeField] private PieceSettings _pieceSettings;

        private readonly Dictionary<EPiece, PieceProperties> _piecesProperties;

        private void Awake()
        {
            InitPool(_piecePrefab);
            foreach (var pieceProperties in _pieceSettings.PiecesProperties)
            {
                _piecesProperties.TryAdd(pieceProperties.Type,pieceProperties);
            }
        }
    
        public PieceEntity GetPiece(EPiece pieceType)
        {
            var tile = Get();
            tile.SetTile(pieceType, _piecesProperties[pieceType].Sprite);
            return tile;
        }

        public void ReleasePiece(PieceEntity tile)
        {
            Release(tile);
        }
    }
}
