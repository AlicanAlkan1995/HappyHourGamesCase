using System.Collections.Generic;
using HappyHourGames.Managers.Board;
using HappyHourGames.Managers.Tile;
using UnityEngine;

namespace HappyHourGames.Managers.ItemMovement
{
    public class ItemMovementService : IItemMovementService
    {
        private readonly IBoardService _boardService;
        private readonly ITileController _itemControllerService;
        private readonly IItemDatabase _itemDatabase;

        private int []_rowDirections = { -1, 0, 1, 0 };
        private int []_colDirections = { 0, 1, 0, -1 };
        
        public ItemMovementService(IBoardService boardService, ITileController itemControllerService,ItemDatabase database)
        {
            _boardService = boardService;
            _itemControllerService = itemControllerService;
            _itemDatabase = database;
        }

        public bool IsPositionValid(Vector2Int position)
        {
            return position.x >= 0 && position.x < _boardService.Width && position.y >= 0 && position.y < _boardService.Height;
        }
        public bool CanMoveItem(IMovable item, Vector2Int newPosition)
        {
            return CanPlaceItemAtPosition(newPosition, item.Size);
        }
        
        public bool CanPlaceItem(string itemId, Vector2Int position)
        {
            IItemData itemData = _itemDatabase.GetItemByID(itemId);
            if (itemData == null)
            {
                Debug.LogError($"Item with ID '{itemId}' not found in the pool.");
                return false;
            }

            return CanPlaceItemAtPosition(position, itemData.Size);
        }
        

        private bool CanPlaceItemAtPosition(Vector2Int position, Vector2Int itemSize)
        {
            for (int x = 0; x < itemSize.x; x++)
            {
                for (int y = 0; y < itemSize.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y);
                    
                    if (_boardService.GetTileAt(tilePos) == null)
                    {
                        Debug.LogWarning($"Tile at position {tilePos} is null.");
                        return false;
                    }

                    if (_itemControllerService.IsTileOccupied(tilePos))
                    {
                        Debug.LogWarning($"Tile at position {tilePos} is already occupied.");
                        return false;
                    }
                }
            }
            return true;
        }
        
        public bool CanPlaceMoveAtPosition(Vector2Int position)
        {
            if (_boardService.GetTileAt(position) == null)
                return false;
            if (_itemControllerService.IsTileOccupied(position))
                return false;
            
            return true;
        }

    


        public void PlaceItem(Vector2Int position, string itemId)
        {
            if (CanPlaceItem(itemId, position))
            {
                IItemData itemData = _itemDatabase.GetItemByID(itemId);
                _itemControllerService.PlaceItem(position, itemData);
            }
            else
            {
                Debug.Log($"Cannot place item with ID '{itemId}' at position {position}.");
            }
        }
        public Vector3 GetRandomPosition()
        {
            const int maxAttempts = 100;

            Vector3 position = Vector3.one;
            for (int attempts = 0; attempts < maxAttempts; attempts++)
            {
                Vector2Int size = Vector2Int.one;
                // Generate a random position.
                Vector3 randomPos = new Vector3(
                    Random.Range(0, _boardService.Width - size.x + 1),
                    Random.Range(0, _boardService.Height - size.y + 1),
                    0
                );

                // Check if the item can be placed at this position.
                if (CanPlaceItemAtPosition(new Vector2Int((int)randomPos.x,(int)randomPos.y), size))
                {
                    position =  randomPos;

                }
            }

            return position;
        }

        public List<Vector3> GetRandomPositions(int count)
        {
            List<Vector3> list = new List<Vector3>();
            for (int attempts = 0; list.Count < count; attempts++)
            {
                Vector2Int size = Vector2Int.one;
                // Generate a random position.
                Vector3 randomPos = new Vector3(
                    Random.Range(0, _boardService.Width - size.x + 1),
                    Random.Range(0, _boardService.Height - size.y + 1),
                    0
                );

                // Check if the item can be placed at this position.
                if (CanPlaceItemAtPosition(new Vector2Int((int)randomPos.x,(int)randomPos.y), size))
                {
                    if(!list.Contains(randomPos))
                        list.Add(randomPos);

                }
            }

            return list;
        }
        public void PlaceRandomly(List<IItemData> data, int maxAttempts = 100)
        {
            for (int i = 0; i < data.Count; i++)
            {
                IItemData itemData = data[0];
                if (itemData == null)
                {
                    Debug.LogError("No item was fetched from the database.");
                    continue; // Continue to the next iteration of the for-loop to try for the next item.
                }

                bool isPlaced = false; // To check if the item was placed successfully

                for (int attempts = 0; attempts < maxAttempts; attempts++)
                {
                    // Generate a random position.
                    Vector2Int randomPos = new Vector2Int(
                        Random.Range(0, _boardService.Width - itemData.Size.x + 1),
                        Random.Range(0, _boardService.Height - itemData.Size.y + 1)
                    );

                    // Check if the item can be placed at this position.
                    if (CanPlaceItem(itemData.ID,randomPos))
                    {
                        PlaceItem(randomPos,itemData.ID);
                        isPlaced = true; // Mark the item as successfully placed
                        break; // Exit from the attempts loop since the item was placed.
                    }
                }

                // Log warning only if the item couldn't be placed after all attempts.
                if (!isPlaced)
                {
                    Debug.LogWarning($"Could not place item with name '{itemData.ID}' after {maxAttempts} attempts.");
                }
            }
        }

        public Vector2Int FindClosestEmptyTile(Item targetItem, Vector2Int startingGrid)
        {
            if (!IsPositionValid(startingGrid)) return startingGrid;
            
            bool[,] visitedGrids = new bool[_boardService.Width, _boardService.Height];
            
            // Stores indices of the matrix cells
            Stack<Vector2Int> q = new Stack<Vector2Int>();
 
            // Mark the starting cell as visited
            // and push it into the queue
            q.Push(startingGrid);
            visitedGrids[startingGrid.x, startingGrid.y] = true;
 
            // Iterate while the queue
            // is not empty
            while (q.Count != 0)
            {
                Debug.LogWarning($"[ItemMovementService] - Searching closest empty tile");
                
                Vector2Int visitedGrid = q.Pop();
 
                // Go to the adjacent cells
                for(int i = 0; i < 4; i++) 
                {
                    Vector2Int adjustedGridPos = new Vector2Int(visitedGrid.x + _rowDirections[i], visitedGrid.y + _colDirections[i]);
                    if (!IsPositionValid(adjustedGridPos) || visitedGrids[adjustedGridPos.x, adjustedGridPos.y]) continue;
                    
                    if (!CanMoveItem(targetItem, adjustedGridPos))
                    {
                        q.Push(adjustedGridPos);
                        visitedGrids[adjustedGridPos.x, adjustedGridPos.y] = true;
                    }else
                    {
                        q.Clear();
                        Debug.LogWarning($"[ItemMovementService] - Empty tile found {adjustedGridPos}");
                        return adjustedGridPos;
                    }
                }
            }

            return startingGrid;
        }
    }
}

