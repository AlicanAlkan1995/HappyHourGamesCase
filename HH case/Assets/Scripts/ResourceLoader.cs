using UnityEngine;

namespace HappyHourGames.Scripts.Utils
{
    public static class ResourceLoader
    {
        /// <summary>
        /// Loads pop-up GameObject according to given prefab name.
        /// </summary>
        /// <param name="prefabName">Given GameObject prefab name</param>
        /// <returns></returns>
        public static GameObject LoadUIPanel(string prefabName)
        {
            var targetPopup = Resources.Load("UI/" + prefabName) as GameObject;
            return targetPopup;
        }
        
        /// <summary>
        /// Loads pop-up GameObject according to given prefab name.
        /// </summary>
        /// <param name="prefabName">Given GameObject prefab name</param>
        /// <returns></returns>
        public static GameObject LoadPopup(string prefabName)
        {
            var targetPopup = Resources.Load("Popups/" + prefabName) as GameObject;
            return targetPopup;
        }
        
        /// <summary>
        /// Loads Pop-ups Canvas.
        /// </summary>
        /// <returns></returns>
        public static GameObject LoadPopupCanvas()
        {
            var popupCanvas = Resources.Load("PopupCanvas") as GameObject;
            return popupCanvas;
        }

        public static GameObject LoadRaycastBlocker()
        {
            var raycastBlocker = Resources.Load<GameObject>("UI/RaycastBlocker");
            return raycastBlocker;
        }
        
    }
}