﻿using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Board
{
    public class TileSpawner : PoolerBase<TileEntity>
    {
        [SerializeField] private TileEntity _tilePrefab;
        
        private void Awake()
        {
            InitPool(_tilePrefab);
        }
        
        public TileEntity GetTile()
        {
            var tileBackground = Get();
            return tileBackground;
        }

        public void ReleaseTile(TileEntity tileBackground)
        {
            Release(tileBackground);
        }
    }
}