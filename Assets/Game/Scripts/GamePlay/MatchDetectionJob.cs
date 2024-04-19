using Game.Scripts.Piece;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Game.Scripts.GamePlay
{
    [BurstCompile]
    public struct MatchDetectionJob : IJob
    {
        [ReadOnly]
        public NativeArray<EPiece> Board;
        public int BoardWidth;
        public int BoardHeight;

        [WriteOnly]
        public NativeArray<bool> MatchPositions;

        [BurstCompile]
        public void Execute()
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    int index = y * BoardWidth + x;
                    EPiece currentTileType = Board[index];

                    // Check horizontal matches
                    int horizontalCount = CheckMatches(new int2(x, y), new int2(1, 0), currentTileType);

                    // Check vertical matches
                    int verticalCount = CheckMatches(new int2(x, y), new int2(0, 1), currentTileType);

                    // If there's a match in either direction, add the current position to the list
                    if (horizontalCount >= 3)
                    {
                        for (int i = 0; i < horizontalCount; i++)
                        {
                            MatchPositions[y * BoardWidth + (x + i)] = true;
                            //MatchPositions.Add(new int2(x - i, y));
                        }
                    }

                    if (verticalCount >= 3)
                    {
                        for (int i = 0; i < verticalCount; i++)
                        {
                            MatchPositions[(y + i) * BoardWidth + x] = true;
                            //MatchPositions.Add(new int2(x, y - i));
                        }
                    }
                }
            }
        }

        [BurstCompile]
        private int CheckMatches(int2 startPosition, int2 direction, EPiece targetType)
        {
            int count = 0;
            int2 currentPosition = startPosition;

            while (IsValidPosition(currentPosition) && Board[currentPosition.y * BoardWidth + currentPosition.x] == targetType)
            {
                count++;
                currentPosition += direction;
            }

            return count;
        }

        [BurstCompile]
        private bool IsValidPosition(int2 position)
        {
            return position.x >= 0 && position.x < BoardWidth && position.y >= 0 && position.y < BoardHeight;
        }
    }
}