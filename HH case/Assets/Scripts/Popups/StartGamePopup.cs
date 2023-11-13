using System;
using HappyHourGames.Scripts.UI;
using HappyHourGames.UI;
using HappyHourGames.UI.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGameDefinition : PopupDefinition
{
    public Action callBack;

    public StartGameDefinition(Action CallBack) : base("StartGame")
    {
        callBack = CallBack;
    }
}
public class StartGamePopup : BasePopup
{
    private StartGameDefinition _startGameDefinitionDefinition;
    
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI waitingtext;
    private bool isReady = false;
    private int ReadyCount = 0;
    
    public override void Initialize(UIManager givenUIManager, PopupDefinition popupDefinition)
    {
        base.Initialize(givenUIManager, popupDefinition);
            
        _startGameDefinitionDefinition = popupDefinition as StartGameDefinition;
        
        _photonView.viewID = 91919;
        
        if (_startGameDefinitionDefinition == null) return;

    }
    public override void InitFromDefinition(PopupDefinition popupDefinition)
    {
        base.InitFromDefinition(popupDefinition);
            
        _startGameDefinitionDefinition = popupDefinition as StartGameDefinition;
        
        //temporary viewId update with instantiate
        _photonView.viewID = 91919;
        
        if (_startGameDefinitionDefinition == null) return;
        
    }

    [PunRPC]
    void SetReady()
    {
        ReadyCount++;
        if (ReadyCount == PhotonNetwork.room.PlayerCount)
        {
            _startGameDefinitionDefinition.callBack?.Invoke();
            _startGameDefinitionDefinition.Result = 1;
            base.Confirm();
        }
    }

    public override void Confirm()
    {
        if (!isReady)
        {
            readyButton.gameObject.SetActive(false);
            _photonView.RPC("SetReady", PhotonTargets.All);
            isReady = true;
            waitingtext.gameObject.SetActive(true);
        }
         
    }
    
    public override void Cancel()
    {
        _startGameDefinitionDefinition.Result = 2;
        base.Cancel();
    }
}
