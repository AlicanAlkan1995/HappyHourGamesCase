using HappyHourGames.Object_Pooling;
using UnityEngine;

namespace HappyHourGames.Managers.Tile
{
    public interface IItemData
    {
        public PoolObjectType ObjectType { get; }
        public string ID { get; }
        public Vector2Int Size { get; }
        
    }

}
