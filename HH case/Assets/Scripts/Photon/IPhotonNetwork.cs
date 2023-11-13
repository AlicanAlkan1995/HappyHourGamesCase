using HappyHourGames.Object_Pooling;
using HappyHourGames.Scripts.Services;
using UnityEngine;

namespace HappyHourGames.Scripts.Network
{
    public interface IPhotonNetwork 
    {
        public bool IsMasterClient();

        public void SetRoomClosed();

        public int RoomMemberCount();

        public int PlayerNumber();

        public void InstantiateNetworkObject(GameObject photonObject, object[] syncComponents,
            PoolObjectType objectType, Item item);
        
        void Init();

        void OnSystemsInitialized(ServiceLocator serviceLocator);

        public PhotonPlayer[] GetOtherPlayers();
        public PhotonPlayer GetPlayer();

        public void setPlayerState(bool state);

    }
}

