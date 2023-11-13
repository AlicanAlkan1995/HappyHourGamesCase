using System.Collections.Generic;
using HappyHourGames.Managers.Tile;
using UnityEngine;

namespace HappyHourGames.Managers.ItemMovement
{
    public interface IItemMovementService
    {
        bool CanMoveItem(IMovable item, Vector2Int newPosition);
        void PlaceItem(Vector2Int position, string id);

        Vector2Int FindClosestEmptyTile(Item item, Vector2Int startingPosition);
        
        public bool CanPlaceMoveAtPosition(Vector2Int position);

        public Vector3 GetRandomPosition();
        public List<Vector3> GetRandomPositions(int count);

        public void PlaceRandomly(List<IItemData> data, int maxAttempts = 100);
    }
}
