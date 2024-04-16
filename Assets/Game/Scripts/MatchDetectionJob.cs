using Game.Scripts.Tile;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public struct MatchDetectionJob : IJob
{
    [ReadOnly]
    public NativeArray<ETile> board; // The game board

    public int boardWidth; // Width of the game board
    public int boardHeight; // Height of the game board

    [WriteOnly]
    public NativeList<int2> matchPositions; // List to store the positions of matched tiles

    [BurstCompile]
    public void Execute()
    {
        for (int y = 0; y < boardHeight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                int index = y * boardWidth + x;
                ETile currentTileType = board[index];

                // Check horizontal matches
                int horizontalCount = CheckMatches(new int2(x, y), new int2(1, 0), currentTileType);

                // Check vertical matches
                int verticalCount = CheckMatches(new int2(x, y), new int2(0, 1), currentTileType);

                // If there's a match in either direction, add the current position to the list
                if (horizontalCount >= 3)
                {
                    for (int i = 0; i < horizontalCount; i++)
                    {
                        matchPositions.Add(new int2(x - i, y));
                    }
                }

                if (verticalCount >= 3)
                {
                    for (int i = 0; i < verticalCount; i++)
                    {
                        matchPositions.Add(new int2(x, y - i));
                    }
                }
            }
        }
    }

    [BurstCompile]
    private int CheckMatches(int2 startPosition, int2 direction, ETile targetType)
    {
        int count = 0;
        int2 currentPosition = startPosition;

        while (IsValidPosition(currentPosition) && board[currentPosition.y * boardWidth + currentPosition.x] == targetType)
        {
            count++;
            currentPosition += direction;
        }

        return count;
    }

    [BurstCompile]
    private bool IsValidPosition(int2 position)
    {
        return position.x >= 0 && position.x < boardWidth && position.y >= 0 && position.y < boardHeight;
    }
}