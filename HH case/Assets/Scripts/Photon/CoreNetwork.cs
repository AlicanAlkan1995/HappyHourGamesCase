using System.Collections.Generic;
using System.Linq;
using HappyHourGames.Managers.Tile;
using HappyHourGames.Object_Pooling;
using HappyHourGames.Scripts.Network;
using HappyHourGames.Scripts.Services;
using UnityEngine;

public class CoreNetwork : IPhotonNetwork
{
    private ITileController _itemControllerService;
    private IPhotonNetwork _photonNetworkImplementation;

    public bool IsMasterClient() => PhotonNetwork.isMasterClient;
    public void Init()
    {
        PhotonNetwork.ConnectUsingSettings(Application.version);
        
        PhotonNetwork.OnEventCall += OnEvent;
        
        if (IsMasterClient())
        {
            SetRoomClosed();
        }

        PhotonNetwork.InstantiateInRoomOnly = true;
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void OnSystemsInitialized(ServiceLocator serviceLocator)
    {
        _itemControllerService = serviceLocator.GetService<ITileController>();
    }

    public void setPlayerState(bool state)
    {
        PhotonNetwork.player.CustomProperties["IsPlayerReady"] = true;
    }

    public PhotonPlayer[] GetOtherPlayers()
    {
        return PhotonNetwork.otherPlayers;
    }

    public PhotonPlayer GetPlayer()
    {
        return PhotonNetwork.player;
    }

    public void SetRoomClosed()
    {
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        
        Debug.LogError($"Room closed and {PhotonNetwork.room.PlayerCount} in Room");
    }

    public int RoomMemberCount() => PhotonNetwork.room.PlayerCount;

    public int PlayerNumber()
    {
        var playerList = PhotonNetwork.playerList;
        playerList = playerList.OrderByDescending(d => d.NickName).ToArray();
        
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].NickName == PhotonNetwork.player.NickName)
                return i;
        }

        return -1;
    }

    public void InstantiateNetworkObject(GameObject photonObject, object[] syncComponents,PoolObjectType objectType,Item item)
    {
        var photonView = photonObject.AddComponent<PhotonView>();
        
        photonView.viewID = PhotonNetwork.AllocateViewID();
        photonView.synchronization = ViewSynchronization.UnreliableOnChange;
        photonView.onSerializeTransformOption = OnSerializeTransform.OnlyPosition;
        photonView.ObservedComponents = new List<Component>() { item };
        
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };

        syncComponents[1] = photonView.viewID;

        PhotonNetwork.RaiseEvent((byte)objectType, syncComponents, true, raiseEventOptions);
    }
    
    public void OnEvent(byte eventCode, object objectsArray, int c)
    {
        object[] data = (object[])objectsArray;
        PoolObjectType objectType = (PoolObjectType)eventCode;
        switch (objectType)
        {
            case PoolObjectType.Character:
            {
                GameObject gameObject = PoolManager.Instance.GetPoolObject(objectType);
                PhotonView  photonView = gameObject.AddComponent<PhotonView>();
        
                photonView.synchronization = ViewSynchronization.UnreliableOnChange;
                photonView.onSerializeTransformOption = OnSerializeTransform.OnlyPosition;
        
                Item item = gameObject.GetComponent<Item>();

                Vector3 pos = (Vector3)data[0];
                Vector3 size = (Vector3)data[3];

                Vector2Int gridPos = new Vector2Int((int)pos.x, (int)pos.y);
                Vector2Int itemSize = new Vector2Int((int)size.x, (int)size.y);
     
                gameObject.transform.position = pos;

                item.GridPlacement = gridPos;
                item.Size = itemSize;
        
                photonView.ObservedComponents = new List<Component>() { item };
                photonView.viewID = (int) data[1];

                item.UpdateItemName(data[4].ToString());
                
                _itemControllerService.RegisterItem(gridPos,new BasicItem( data[2].ToString(),itemSize,objectType));
                break;
            }
            case PoolObjectType.Stone:
            {
                GameObject gameObject = PoolManager.Instance.GetPoolObject(objectType);
                PhotonView  photonView = gameObject.AddComponent<PhotonView>();
                
                Item item = gameObject.GetComponent<Item>();

                Vector3 pos = (Vector3)data[0];
                Vector3 size = (Vector3)data[3];

                Vector2Int gridPos = new Vector2Int((int)pos.x, (int)pos.y);
                Vector2Int itemSize = new Vector2Int((int)size.x, (int)size.y);
     
                gameObject.transform.position = pos;

                item.GridPlacement = gridPos;
                item.Size = itemSize;
        
                photonView.ObservedComponents = new List<Component>() { item };
                photonView.viewID = (int) data[1];

                _itemControllerService.RegisterItem(gridPos,new BasicItem( data[2].ToString(),itemSize,objectType));
                break;
            }
            case PoolObjectType.Wood:
            {
                GameObject gameObject = PoolManager.Instance.GetPoolObject(objectType);
                PhotonView  photonView = gameObject.AddComponent<PhotonView>();
                
                Item item = gameObject.GetComponent<Item>();

                Vector3 pos = (Vector3)data[0];
                Vector3 size = (Vector3)data[3];

                Vector2Int gridPos = new Vector2Int((int)pos.x, (int)pos.y);
                Vector2Int itemSize = new Vector2Int((int)size.x, (int)size.y);
     
                gameObject.transform.position = pos;

                item.GridPlacement = gridPos;
                item.Size = itemSize;
        
                photonView.ObservedComponents = new List<Component>() { item };
                photonView.viewID = (int) data[1];

                _itemControllerService.RegisterItem(gridPos,new BasicItem( data[2].ToString(),itemSize,objectType));
                break;
            }
        }
        
        
    }
    
}
