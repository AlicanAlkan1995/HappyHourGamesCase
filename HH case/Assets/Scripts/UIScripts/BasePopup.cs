using DG.Tweening;
using HappyHourGames.Scripts.UI;
using UnityEngine;

namespace HappyHourGames.UI.Popups
{
    public class BasePopup : MonoBehaviour, IPopup
    {
        
        private PopupDefinition _popupDefinition;
        public UIManager uiManager;
        /// <summary>
        /// Initialize Popup
        /// </summary>
        /// <param name="givenUIManager">Main UI Manager</param>
        /// <param name="popupDefinition">Panel Definition</param>
        public virtual void Initialize(UIManager givenUIManager, PopupDefinition popupDefinition)
        {
            uiManager = givenUIManager;
            _popupDefinition = popupDefinition;

            Show();
        }

        /// <summary>
        /// Inits Popup from Given Definition
        /// </summary>
        /// <param name="popupDefinition">Panel Definition</param>
        public virtual void InitFromDefinition(PopupDefinition popupDefinition)
        {
            _popupDefinition = popupDefinition;

            Show();
        }

        /// <summary>
        /// Shows Popup
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
            transform.DOScale(Vector2.one, .25f);
        }

        /// <summary>
        /// Shows Popup
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Confirms Popup
        /// </summary>
        public virtual void Confirm()
        {
            transform.DOScale(Vector2.zero, .15f).OnComplete(() =>
            {
                uiManager.ClosePopup(this);
                _popupDefinition.Result = 1;
            });
        }

        /// <summary>
        /// Cancels Popup
        /// </summary>
        public virtual void Cancel()
        {
            transform.DOScale(Vector2.zero, .15f).OnComplete(() =>
            {
                uiManager.ClosePopup(this);
                _popupDefinition.Result = 2;
            });
        }

        /// <summary>
        /// Closes Popup
        /// </summary>
        public virtual void Close()
        {
            transform.DOScale(Vector2.zero, .15f).OnComplete(() =>
            {
                uiManager.ClosePopup(this);
                _popupDefinition.Result = 2;
            });
        }
    }
}
