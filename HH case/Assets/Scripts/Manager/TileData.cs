using HappyHourGames.Object_Pooling;
using UnityEngine;

namespace HappyHourGames.Managers.Board
{
    /// <summary>
    /// this is data representation of a tile on the board
    /// </summary>
    public class TileData
    {
        public PoolObjectType ObjectType;
        public Vector2Int Position { get; set; }
    }
}