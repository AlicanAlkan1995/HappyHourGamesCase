using System.Collections.Generic;
using UnityEngine;

namespace HappyHourGames.Managers.Tile
{
    public interface IMovable
    {
        Vector2Int Size { get; }
        public Vector2Int GridPlacement { get; set; }
        public Vector2Int _latestGridPos { get; set; }
        void SetPosition(Vector2 newPosition);
        Transform GetTransform();
        void MoveTo(Vector2Int newPosition);
        bool IsOwnedByMe { get; set; }
        bool IsNetworkObject();
        void MovePath(List<Vector2Int> path);
        void UpdateItemName(string name);
        Color Color { get; set; }

    }
}