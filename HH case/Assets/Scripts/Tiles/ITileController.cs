using System;
using UnityEngine;

namespace HappyHourGames.Managers.Tile
{
    public interface ITileController
    {
        bool IsTileOccupied(Vector2Int position);
        void PlaceItem(Vector2Int position, IItemData itemData);
        void RemoveItem(Vector2Int position);
        IItemData GetItemDataAt(Vector2Int position);
        void RemoveItem(IMovable item);
        void UpdateItemPosition(IMovable item, Vector2Int newPosition);

        event Action<Vector2Int, IItemData> OnItemPlaced;
        
        event Action<Vector2Int> OnItemRemoved;

        void OnItemMoved(Item it, Vector2Int po);
        void RegisterItem(Vector2Int position, IItemData itemData);
    }
}
