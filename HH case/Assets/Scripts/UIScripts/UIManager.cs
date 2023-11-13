using UnityEngine;
using System.Collections.Generic;
using HappyHourGames.Scripts.Utils;
using HappyHourGames.UI;
using HappyHourGames.UI.Popups;
using Image = UnityEngine.UI.Image;

namespace HappyHourGames.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [SerializeField] private Image popupCanvasImage;
        [SerializeField] private CanvasGroup popupCanvasGroup;

        private string _currentSceneName;
        private readonly Dictionary<string, IPopup> _popups = new Dictionary<string, IPopup>();

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            }
            
            DontDestroyOnLoad(this);
        }
        
        /// <summary>
        /// Creates Popup by the Given Popup Definition
        /// </summary>
        /// <param name="popupDefinition"></param>
        public void CreatePopup(PopupDefinition popupDefinition)
        {
            var loadedPopup = ResourceLoader.LoadPopup(popupDefinition.PrefabName);
            var popupGameObject = Instantiate(loadedPopup, popupCanvasGroup.transform);
            
            var iPopup = popupGameObject.GetComponent<IPopup>();
        
            iPopup.Initialize(this, popupDefinition);

            _popups.Add(popupDefinition.PrefabName ,iPopup);
            
            iPopup.Close();
        }

        /// <summary>
        /// Shows Popup by the Given Popup Definition.Loads Popups from Resources or Loads from Popups Dictionary
        /// </summary>
        /// <param name="popupDefinition"></param>
        /// <param name="raycastBlocker"></param>
        public void ShowPopup(PopupDefinition popupDefinition, bool raycastBlocker = true)
        {
            if (_popups.ContainsKey(popupDefinition.PrefabName))
            {
                _popups[popupDefinition.PrefabName].InitFromDefinition(popupDefinition);
            }
            else
            {
                var loadedPopup = ResourceLoader.LoadPopup(popupDefinition.PrefabName);
                var popupGameObject = Instantiate(loadedPopup, popupCanvasGroup.transform);
                
                var iPopup = popupGameObject.GetComponent<IPopup>();
            
                iPopup.Initialize(this, popupDefinition);

                _popups.Add(popupDefinition.PrefabName ,iPopup);
            }
            
            popupCanvasGroup.alpha = 1;
            popupCanvasGroup.interactable = true;
            popupCanvasGroup.blocksRaycasts = true;
            popupCanvasImage.enabled = raycastBlocker;
        }
        
        /// <summary>
        /// Closes Open Popup
        /// </summary>
        public void ClosePopup(BasePopup popup)
        {
            popup.Hide();
            
            popupCanvasGroup.alpha = 0;
            popupCanvasGroup.interactable = false;
            popupCanvasGroup.blocksRaycasts = false;
        }
        
    }
}
