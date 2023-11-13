using HappyHourGames.Managers.Tile;
namespace HappyHourGames.Managers.ItemMovement
{
    public interface IItemDatabase
    {
        void AddItem(IItemData item);
        IItemData GetItemByID(string id);
        IItemData GetRandomItem();
    }
}