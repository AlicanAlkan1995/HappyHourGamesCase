using UnityEngine;

namespace HappyHourGames.Scripts.PahtFinding
{
    public class Node
    {
        public Vector2Int position;
        public Node parent;
        public int g;
        public int h;
        public int f;

        public Node(Vector2Int pos, Node parentNode = null)
        {
            position = pos;
            parent = parentNode;
            g = 0;
            h = 0;
            f = 0;
        }
    }
}
