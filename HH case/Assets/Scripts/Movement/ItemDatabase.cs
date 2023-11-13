using System.Collections.Generic;
using System.Linq;
using HappyHourGames.Managers.Tile;

namespace HappyHourGames.Managers.ItemMovement
{
    public class ItemDatabase : IItemDatabase
    {
        private Dictionary<string, IItemData> _items;

        public ItemDatabase()
        {
            _items = new Dictionary<string, IItemData>();
        }

        public void AddItem(IItemData item)
        {
            _items.TryAdd(item.ID, item);
        }

        
        public IItemData GetItemByID(string id)
        {
            _items.TryGetValue(id, out IItemData item);
            return item;
        }

        public IItemData GetRandomItem()
        {
            if (_items.Count == 0)
            {
                return null; // Return null if there are no items in the database.
            }

            int randomIndex = UnityEngine.Random.Range(0, _items.Count);
            return _items.ElementAt(randomIndex).Value;
        }
        
        // Further methods like RemoveItem, UpdateItem, etc. 
    }

}
