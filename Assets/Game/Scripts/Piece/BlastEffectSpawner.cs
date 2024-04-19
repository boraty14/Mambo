using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Piece
{
    public class BlastEffectSpawner : PoolerBase<BlastEffectEntity>
    {
        [SerializeField] private BlastEffectEntity _blastEffectPrefab;
        [SerializeField] private PieceSettings _pieceSettings;
        
        private void Awake()
        {
            InitPool(_blastEffectPrefab);
        }

        public async UniTaskVoid PlayBlastEffect(PieceEntity piece)
        {
            var blastEffect = Get();
            blastEffect.SetEffect(piece);
            await UniTask.Delay(TimeSpan.FromSeconds(_pieceSettings.BlastDuration));
            Release(blastEffect);
        } 
    }
}