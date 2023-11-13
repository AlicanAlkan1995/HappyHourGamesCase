using System.Collections;
using HappyHourGames.MatchMaker;
using HappyHourGames.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonNetworkManager : Photon.PunBehaviour, IPunCallbacks
{
    [SerializeField] private TextMeshProUGUI connectionInfoText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button forceStartButton;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Button updateNicknameButton;
    
    private readonly IMatchMakingService _matchMakingService = new MatchMaker();
    
    public virtual void Start()
    {
        Application.targetFrameRate = 60;
        PhotonNetwork.ConnectUsingSettings(Application.version);
        PhotonNetwork.automaticallySyncScene = true;
        
        if(PlayerPrefs.HasKey("PlayerName"))
            PhotonNetwork.playerName = PlayerPrefs.GetString("PlayerName");
        else
        {
            ShowUpdateNicknamePopup();
        }

        infoText.text = $"Nickname: {PlayerPrefs.GetString("PlayerName")}";
        infoText.gameObject.SetActive(true);
        
        updateNicknameButton.onClick.AddListener(ShowUpdateNicknamePopup);
    }
    
    private async void ShowUpdateNicknamePopup()
    {
        var updateNickname = new UpdateNickameDefinition(PlayerPrefs.GetString("PlayerName"));
        UIManager.Instance.ShowPopup(updateNickname);
        await updateNickname.PopupResult();
        PhotonNetwork.playerName = PlayerPrefs.GetString("PlayerName");
        infoText.text = $"Nickname: {PlayerPrefs.GetString("PlayerName")}";
       
    }
    private void FindRoom()
    {
        if (!PlayerPrefs.HasKey("PlayerName"))
        {
            ShowUpdateNicknamePopup();
            return;
        }
          
        Debug.Log("quickMatch");
        
        startButton.onClick.RemoveListener(FindRoom);
        startButton.gameObject.SetActive(false);
        updateNicknameButton.gameObject.SetActive(false);
        
       
        _matchMakingService.QuickMatch();
    }

    private IEnumerator StartGame()
    {
        while (PhotonNetwork.inRoom)
        {
            yield return new WaitForSeconds(1);
            connectionInfoText.text = $"Searching {GetConnectingDots()}";
            infoText.text = $"Nickname: {PlayerPrefs.GetString("PlayerName")} \n Waiting for opponent \n {PhotonNetwork.room.PlayerCount} / {PhotonNetwork.room.MaxPlayers}";
            if (PhotonNetwork.room.PlayerCount == PhotonNetwork.room.MaxPlayers)
            {
                infoText.text = "Game Starting";
                
                yield return new WaitForSeconds(3);
                
                PhotonNetwork.LoadLevel("GameScene");
            }
        }
    }


    #region PhotonCallBacks

    private new void OnConnectedToMaster()
    {
        connectionInfoText.text = "Connected";
        
        updateNicknameButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(FindRoom);
        startButton.gameObject.SetActive(true);
        
        Debug.Log($"Connected");
        PhotonNetwork.JoinLobby();
    }

    public new void OnDisconnectedFromPhoton()
    {
        startButton.onClick.RemoveListener(FindRoom);
        startButton.gameObject.SetActive(false);
        updateNicknameButton.gameObject.SetActive(false);
        Debug.Log($"Disconnected");
    }
    

    private new void OnJoinedRoom()
    {
        StartCoroutine(StartGame());

        if (PhotonNetwork.isMasterClient)
        {
            forceStartButton.gameObject.SetActive(true);
            forceStartButton.onClick.AddListener(ForceStartGame);
        }
        
        Debug.Log($"Joined Room");
    }

    private void ForceStartGame()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    private new void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        startButton.onClick.AddListener(FindRoom);
        startButton.gameObject.SetActive(true);
        updateNicknameButton.gameObject.SetActive(true);
        
        Debug.Log($"Joined Room Failed");
    }

    private new void OnLeftRoom()
    {
        forceStartButton.gameObject.SetActive(false);
        
        startButton.onClick.AddListener(FindRoom);
        startButton.gameObject.SetActive(true);
        updateNicknameButton.gameObject.SetActive(true);
        
        connectionInfoText.text = PhotonNetwork.room.Name;
        
        Debug.Log($"Left Room");
    }

    public new void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        _matchMakingService.QuickMatch();
    }

    #endregion
    
    string GetConnectingDots()
    {
        string str = "";
        int numberOfDots = Mathf.FloorToInt( Time.timeSinceLevelLoad * 3f % 4 );

        for( int i = 0; i < numberOfDots; ++i )
        {
            str += " .";
        }

        return str;
    }
}
