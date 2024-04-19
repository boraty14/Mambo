﻿using System;
using Game.Scripts.Board;
using Game.Scripts.Piece;

namespace Game.Scripts
{
    public static class EventBus
    {
        public static void StartGame() => OnStartGame?.Invoke();
        public static event Action OnStartGame;

        public static void EndGame() => OnEndGame?.Invoke();
        public static event Action OnEndGame;

        public static void SetBoardLevelData(BoardLevelData boardLevelData) => OnSetBoardLevelData?.Invoke(boardLevelData);
        public static event Action<BoardLevelData> OnSetBoardLevelData;
    }
}