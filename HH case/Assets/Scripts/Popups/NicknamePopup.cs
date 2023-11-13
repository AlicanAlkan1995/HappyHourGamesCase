using System;
using HappyHourGames.Scripts.UI;
using HappyHourGames.UI;
using HappyHourGames.UI.Popups;
using Photon;
using TMPro;
using UnityEngine;

[Serializable]
public class UpdateNickameDefinition : PopupDefinition
{
    public readonly string Text;

    public UpdateNickameDefinition(string givenText) : base("NicknamePopup")
    {
        Text = givenText;
    }
}
public class NicknamePopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI currentNickname;
    [SerializeField] private TMP_InputField inputField;
    
    
    private UpdateNickameDefinition _updateNickameDefinition;
    
    public override void Initialize(UIManager givenUIManager, PopupDefinition popupDefinition)
    {
        base.Initialize(givenUIManager, popupDefinition);
            
        _updateNickameDefinition = popupDefinition as UpdateNickameDefinition;
        
        if (_updateNickameDefinition == null) return;
        
        currentNickname.text = _updateNickameDefinition.Text;
 
        
    }
    public override void InitFromDefinition(PopupDefinition popupDefinition)
    {
        base.InitFromDefinition(popupDefinition);
            
        _updateNickameDefinition = popupDefinition as UpdateNickameDefinition;
        
        if (_updateNickameDefinition == null) return;
        
        currentNickname.text = _updateNickameDefinition.Text;

        
    }

    public override void Confirm()
    {
        if (inputField.text == string.Empty)
        {
            Cancel();
            return;
        }
        PlayerPrefs.SetString("PlayerName",inputField.text);
        _updateNickameDefinition.Result = 1;
        base.Confirm();
        
    }
    
    public override void Cancel()
    {
        _updateNickameDefinition.Result = 2;
        base.Cancel();
    }
    
    
}
