using System.Collections.Generic;
using UnityEngine;

namespace HappyHourGames.Scripts.PahtFinding
{
    public interface IPathFinding
    {
        public List<Vector2Int> FindPath(Vector2Int StartPosition, Vector2Int EndPosition);
    }

}
