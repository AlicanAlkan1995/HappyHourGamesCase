using System;
using HappyHourGames.Object_Pooling;
using UnityEngine;

namespace HappyHourGames.Managers.Tile
{
    public class ItemControllerService : ITileController
    {
        private IItemData[,] _itemDatas;
        public event Action<Vector2Int, IItemData> OnItemPlaced;
        public event Action<Vector2Int> OnItemRemoved;
        
        public ItemControllerService(int width, int height)
        {
            _itemDatas = new IItemData[width, height];
            GameActions.OnItemMoved += OnItemMoved;
        }
        public void OnItemMoved(Item item, Vector2Int position)
        {
            Vector2Int oldPosition = item.GridPlacement;
            
            IItemData itemData = GetItemDataAt(oldPosition);
            
            RemoveItemFromData(oldPosition);
            
            item.GridPlacement = position;
            
            PlaceItemInData(position, itemData);
        }
        
        public bool IsTileOccupied(Vector2Int position)
        {
            if (_itemDatas[position.x, position.y] == null)
                return false;
            
            return _itemDatas[position.x, position.y].ObjectType != PoolObjectType.Wood;
        }

        public void RemoveItem(IMovable item)
        {
            Vector2Int position = item.GridPlacement; // Assuming IMovable has a GridPosition property.
            IItemData itemData = _itemDatas[position.x, position.y];
            if (itemData == null) return;

            Vector2Int itemSize = itemData.Size;
            for (int x = 0; x < itemSize.x; x++)
            {
                for (int y = 0; y < itemSize.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y);
                    _itemDatas[tilePos.x, tilePos.y] = null;
                }
            }

            OnItemRemoved?.Invoke(position);
        }

        private void MoveItemData(Item item, Vector2Int newPosition)
        {
            // Remove the item from its old position in the data array
            Vector2Int oldPosition = item.GridPlacement;
            
            IItemData itemData = GetItemDataAt(oldPosition);
            
            RemoveItemFromData(oldPosition);

            PlaceItemInData(newPosition, itemData);
        }
        private void MoveItemInData(IMovable item, Vector2Int newPosition)
        {
            // Remove the item from its old position in the data array
            Vector2Int oldPosition = item.GridPlacement;
            
            IItemData itemData = GetItemDataAt(oldPosition);
            
            RemoveItemFromData(oldPosition);

            PlaceItemInData(newPosition, itemData);
        }

        private void RemoveItemFromData(Vector2Int position)
        {
            IItemData itemData = _itemDatas[position.x, position.y];
            if (itemData == null) return;

            Vector2Int itemSize = itemData.Size;
            for (int x = 0; x < itemSize.x; x++)
            {
                for (int y = 0; y < itemSize.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y);
                    _itemDatas[tilePos.x, tilePos.y] = null;
                }
            }
        }

        private void PlaceItemInData(Vector2Int position, IItemData itemData)
        {
            Vector2Int itemSize = itemData.Size;
            for (int x = 0; x < itemSize.x; x++)
            {
                for (int y = 0; y < itemSize.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y);
                    
                    _itemDatas[tilePos.x, tilePos.y] = itemData;
                }
            }
        }

        public void UpdateItemPosition(IMovable item, Vector2Int newPosition)
        {
            // Update the data
            MoveItemInData(item, newPosition);

            // Update the GridPlacement property
            item.GridPlacement = newPosition;

            // Notify the view
            // OnItemMoved?.Invoke(item, newPosition);
        }
        
        public void PlaceItem(Vector2Int position, IItemData itemData)
        {
            Vector2Int itemSize = itemData.Size;
            for (int x = 0; x < itemSize.x; x++)
            {
                for (int y = 0; y < itemSize.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y);
                    _itemDatas[tilePos.x, tilePos.y] = itemData;
                }
            }
            
            OnItemPlaced?.Invoke(position, itemData);
        }

        public void RegisterItem(Vector2Int position, IItemData itemData)
        {
            Vector2Int itemSize = itemData.Size;
            for (int x = 0; x < itemSize.x; x++)
            {
                for (int y = 0; y < itemSize.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y);
                    _itemDatas[tilePos.x, tilePos.y] = itemData;
                }
            }
        }

        public void RemoveItem(Vector2Int position)
        {
            IItemData itemData = _itemDatas[position.x, position.y];
            if (itemData == null) return;

            Vector2Int itemSize = itemData.Size;
            for (int x = 0; x < itemSize.x; x++)
            {
                for (int y = 0; y < itemSize.y; y++)
                {
                    Vector2Int tilePos = position + new Vector2Int(x, y);
                    _itemDatas[tilePos.x, tilePos.y] = null;
                }
            }

            OnItemRemoved?.Invoke(position);
        }

        public IItemData GetItemDataAt(Vector2Int position)
        {
            return _itemDatas[position.x, position.y];
        }
        
        private void LogItemCount()
        {
            int itemCount = 0;

            foreach (var item in _itemDatas)
            {
                if (item != null)
                {
                    itemCount++;
                }
            }

            Debug.Log($"Total items in _itemDatas: {itemCount}");
        }
}
}

