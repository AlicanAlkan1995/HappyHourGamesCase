using System;
using UnityEngine;

namespace HappyHourGames.Managers.Board
{
    public interface IBoardService
    {
        void InitializeBoard();
        TileData GetTileAt(Vector2Int position);
        void SetTileAt(Vector2Int position, TileData tileData);
        bool IsGroupEmpty(int startX, int startY, int width, int height);
        Vector2Int BoardCenter();
        int Width { get; }
        int Height { get; }
        
        int hole { get; }
        
        event Action OnBoardChanged;

        public Vector2Int[] GetCharacterPosition(int playerNumber);
    }
}
