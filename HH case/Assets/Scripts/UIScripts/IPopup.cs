using HappyHourGames.Scripts.UI;

namespace HappyHourGames.UI.Popups
{
    internal interface IPopup
    {
        /// <summary>
        /// Initialize Popup
        /// </summary>
        /// <param name="uiManager">Main UI Manager</param>
        /// <param name="popupDefinition">Panel Definition</param>
        public void Initialize(UIManager uiManager, PopupDefinition popupDefinition);

        /// <summary>
        /// Inits Popup from Given Definition
        /// </summary>
        /// <param name="popupDefinition">Panel Definition</param>
        public void InitFromDefinition(PopupDefinition popupDefinition);

        /// <summary>
        /// Shows Popup
        /// </summary>
        public void Show();

        /// <summary>
        /// Hides Popup
        /// </summary>
        public void Hide();

        /// <summary>
        /// Confirms Popup
        /// </summary>
        public void Confirm();

        /// <summary>
        /// Cancels Popup
        /// </summary>
        public void Cancel();

        /// <summary>
        /// Closes Popup
        /// </summary>
        public void Close();
    }
}