using System.Threading.Tasks;

namespace HappyHourGames.UI
{
    public class PopupDefinition
    {
        public string PrefabName;
        
        public int Result = 0;
        public int None = 0, Yes = 1, Cancel = 2;

        protected PopupDefinition(string prefabName)
        {
            PrefabName = prefabName;
        }
        
        /// <summary>
        /// Returns Popup Result
        /// </summary>
        public async Task<bool> PopupResult()
        {
            while (Result == None)
            {
                await Task.Yield();
            }

            return Result == Yes;
        }
    }
}