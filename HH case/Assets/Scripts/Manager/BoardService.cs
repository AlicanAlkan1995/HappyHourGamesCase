using System;
using UnityEngine;

namespace HappyHourGames.Managers.Board
{
    public class BoardService : IBoardService
    {
        private const int BOARD_WIDTH = 30;
        private const int BOARD_HEIGHT = 30;
        private const int HOLE = 3; // Size of the hole, e.g., 3x3
        private readonly  Vector2Int boardCenter = new Vector2Int(BOARD_WIDTH / 2 - 1, BOARD_HEIGHT / 2 - 1);
        private readonly TileData[,] _board = new TileData[BOARD_WIDTH, BOARD_HEIGHT];
        public event Action OnBoardChanged;
        
        public Vector2Int BoardCenter() => boardCenter;
        public int Width => BOARD_WIDTH;
        public int Height => BOARD_HEIGHT;

        public int hole => HOLE;

        public void InitializeBoard()
        {
            int holeStartX = (BOARD_WIDTH - HOLE) / 2;
            int holeStartY = (BOARD_HEIGHT - HOLE) / 2;
            int holeEndX = holeStartX + HOLE;
            int holeEndY = holeStartY + HOLE;

            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BOARD_HEIGHT; y++)
                {
                    // Check if the current position is within the hole's boundaries
                    if (x >= holeStartX && x < holeEndX && y >= holeStartY && y < holeEndY)
                    {
                        Debug.Log($"Tile at position {x},{y} is set to null for the hole.");
                        _board[x, y] = null;
                        continue;
                    }

                    // For simplicity, let's create a default tile for every position
                    TileData tileData = new TileData
                    {
                        Position = new Vector2Int(x, y),
                    };
                    _board[x, y] = tileData;
                }
            }

            // Invoke the OnBoardChanged event after initialization.
            OnBoardChanged?.Invoke();
        }

        public Vector2Int[] GetCharacterPosition(int playerNumber)
        {
            int holeStartX = (BOARD_WIDTH - HOLE) / 2;
            int holeStartY = (BOARD_HEIGHT - HOLE) / 2;

            Vector2Int[] positions = new Vector2Int[3];
            switch (playerNumber)
            {
                
                case 0:
                {
                    int slot = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        positions[i] = new Vector2Int(holeStartX + HOLE, holeStartY  + slot);
                        slot++;
                    }
                    break;
                }
                
                case 1:
                {
                    int slot = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        positions[i] = new Vector2Int(holeStartX + slot, holeStartY - 1);
                        slot++;
                    }
                    break;
                }
                
                case 2:
                {
                    int slot = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        positions[i] = new Vector2Int(holeStartX + slot, holeStartY + HOLE);
                        slot++;
                    }
                    break;
                }
                case 3:
                {
                    int slot = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        positions[i] = new Vector2Int(holeStartX + -1, holeStartY + slot );
                        slot++;
                    }
                    break;
                }
            }

            return positions;
        }
        
        
        public TileData GetTileAt(Vector2Int position)
        {
            if (IsPositionValid(position))
            {
                return _board[position.x, position.y];
            }
            return null;
        }
        
        public void SetTileAt(Vector2Int position, TileData tileData)
        {
            if (IsPositionValid(position))
            {
                _board[position.x, position.y] = tileData;
                OnBoardChanged?.Invoke();
            }
        }
        private bool IsPositionValid(Vector2Int position)
        {
            return position.x >= 0 && position.x < BOARD_WIDTH && position.y >= 0 && position.y < BOARD_HEIGHT;
        }
        
        public bool IsGroupEmpty(int startX, int startY, int width, int height)
        {
            if (!IsPositionValid(new Vector2Int(startX, startY)) || !IsPositionValid(new Vector2Int(startX + width - 1, startY + height - 1)))
            {
                return false; // Early return if the provided starting point and ending point of the region are not valid.
            }

            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    if (_board[x, y] != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

