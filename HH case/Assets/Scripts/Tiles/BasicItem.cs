using System.Collections;
using System.Collections.Generic;
using HappyHourGames.Object_Pooling;
using UnityEngine;

namespace HappyHourGames.Managers.Tile
{
    public class BasicItem : IItemData
    {
        public PoolObjectType ObjectType { get; }
        public string ID { get; }
        public Vector2Int Size { get; }

        public BasicItem(string id,Vector2Int size,PoolObjectType objectType)
        {
            ID = id;
            Size = size;
            ObjectType = objectType;
        }
    }
}
