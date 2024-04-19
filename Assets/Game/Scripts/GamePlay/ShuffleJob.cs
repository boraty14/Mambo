using Game.Scripts.Piece;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Game.Scripts.GamePlay
{
    [BurstCompile]
    public struct ShuffleJob : IJob
    {
        public NativeArray<EPiece> Board;
        public int Seed;

        [BurstCompile]
        public void Execute()
        {
            var random = new Random((uint)Seed);

            for (int i = Board.Length - 1; i > 0; i--)
            {
                int j = random.NextInt(i + 1);
                EPiece temp = Board[i];
                Board[i] = Board[j];
                Board[j] = temp;
            }
        }
    }
}