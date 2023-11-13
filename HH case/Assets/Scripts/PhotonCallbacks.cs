using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonCallbacks : IPunCallbacks
{
    public void OnConnectedToPhoton()
    {
        Debug.Log("connectedPhoton");
    }

    public void OnLeftRoom()
    {
        Debug.Log("Left Room");
    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        Debug.Log($"Master Client Switched {newMasterClient.IsMasterClient}");
    }

    public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log($"Create Room Failed");
    }

    public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log($"Join Room Failed");
    }

    public void OnCreatedRoom()
    {
        Debug.Log($"Room Created");
    }

    public void OnJoinedLobby()
    {
        Debug.Log($"Joined Lobby");
    }

    public void OnLeftLobby()
    {
        Debug.Log($"Left Lobby");
    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.Log($"Failed To Connect {cause}");
    }

    public void OnConnectionFail(DisconnectCause cause)
    {
        Debug.Log($"Failed To Connect {cause}");
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected");
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log($"Photon Instantiated {info.photonView.instantiationId}");
    }

    public void OnReceivedRoomListUpdate()
    {
        Debug.Log($"Room List Update");
    }

    public void OnJoinedRoom()
    {
        Debug.Log($"Joined Room");
    }

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log($"New Player Connected {newPlayer.NickName}");
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log($"Player Disconnected {otherPlayer.NickName}");
    }

    public void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log($"Player Random Join Failed");
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("Connected Master");
    }

    public void OnPhotonMaxCccuReached()
    {
        Debug.Log("Max Cccu Reached");
    }

    public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
    {
        Debug.Log("Room Properties Changed");
    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        Debug.Log($"Player Properties Changed {playerAndUpdatedProps.Length}");
    }

    public void OnUpdatedFriendList()
    {
        Debug.Log($"Friend List Updated");
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log($"Custom Auth Failed");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log($"Custom Auth Response");
    }

    public void OnWebRpcResponse(OperationResponse response)
    {
        Debug.Log($"Web Rpc Response {response.DebugMessage}");
    }

    public void OnOwnershipRequest(object[] viewAndPlayer)
    {
        Debug.Log($"Owner Ship Request Received");
    }

    public void OnLobbyStatisticsUpdate()
    {
        Debug.Log($"Lobby Statistics Updated");
    }

    public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
    {
        Debug.Log($"Photon Player Activity Updated");
    }

    public void OnOwnershipTransfered(object[] viewAndPlayers)
    {
        Debug.Log($"Owner Ship Transfered");
    }
}
