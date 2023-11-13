using System.Collections.Generic;
using HappyHourGames.Managers.Tile;
using HappyHourGames.Object_Pooling;
using HappyHourGames.Scripts.Network;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    private Item[,] _itemGameObjects;
    private ITileController _itemControllerService;
    private readonly IPhotonNetwork _photonNetwork = new CoreNetwork();
    private List<Color> colors = new List<Color>()
    {
        Color.blue,
        Color.red,
        Color.yellow,
        Color.magenta,
    };
     
    public void Initialize(ITileController itemControllerService, int width, int height)
    {
        _itemControllerService = itemControllerService;

        _itemGameObjects = new Item[width, height];
        
        // Subscribe to the item placement/removal events
        _itemControllerService.OnItemPlaced += HandleItemPlaced;
        _itemControllerService.OnItemRemoved += HandleItemRemoved;
    }

    private Color RandomColor()
    {
        Color color = colors[^1];
        colors.Remove(colors[^1]);
        return color;
    }
    private void HandleItemPlaced(Vector2Int position, IItemData itemData)
    {
        // Create the new item GameObject
        var itemPoolObject = PoolManager.Instance.GetPoolObject(itemData.ObjectType);
        itemPoolObject.transform.position = CalculateItemPosition(position);
        itemPoolObject.TryGetComponent(out Item item);
        
        if (item == null)
        {
            Debug.LogError("item component not found");
            return;
        }

        item.Size = itemData.Size;
        item.GridPlacement = position;
        item.ObjectType = itemData.ObjectType;
        _itemGameObjects[position.x, position.y] = item;
        item.IsOwnedByMe = true;
        
        if (itemData.ObjectType == PoolObjectType.Character)
        {
            item.Color = RandomColor();
            item.GetSpriteRenderer().color = item.Color;
        }

        item.UpdateItemName(_photonNetwork.GetPlayer().NickName);
        
        if (item.isNetworkObject)
        {
            object[] syncComponents = new object[] { itemPoolObject.transform.position, (int)itemData.ObjectType,itemData.ID,new Vector3(itemData.Size.x,itemData.Size.y,0),_photonNetwork.GetPlayer().NickName};
            _photonNetwork.InstantiateNetworkObject(itemPoolObject,syncComponents,itemData.ObjectType,item);
        }
    }
    private void HandleItemRemoved(Vector2Int position)
    {
        if (_itemGameObjects[position.x, position.y] != null)
        {
            PoolManager.Instance.DestroyObject(_itemGameObjects[position.x, position.y].gameObject,_itemGameObjects[position.x, position.y].ObjectType);
            _itemGameObjects[position.x, position.y] = null;
        }
    }
    private Vector3 CalculateItemPosition(Vector2Int position)
    {
        Vector3 itemPosition = new Vector3(position.x, position.y);
        return itemPosition;
    }
    private void OnDestroy()
    {
        // Unsubscribe from events when this view is destroyed.
        _itemControllerService.OnItemPlaced -= HandleItemPlaced;
        _itemControllerService.OnItemRemoved -= HandleItemRemoved;
    }
}
