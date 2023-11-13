using System.Collections;
using System.Collections.Generic;
using HappyHourGames.Managers.ItemMovement;
using HappyHourGames.Scripts.Services;
using UnityEngine;

namespace HappyHourGames.Scripts.PahtFinding
{
    public class PathFinding :GameService, IPathFinding
    {
        private readonly IItemMovementService _itemMovementService;
        
        public PathFinding(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            _itemMovementService =  serviceLocator.GetService<IItemMovementService>();
        }
        private List<Vector2Int> GetNeighbours(Vector2Int position)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();
            neighbours.Add(new Vector2Int(position.x - 1, position.y));
            neighbours.Add(new Vector2Int(position.x + 1, position.y));
            neighbours.Add(new Vector2Int(position.x, position.y - 1));
            neighbours.Add(new Vector2Int(position.x, position.y + 1));
            return neighbours;
        }
        
        public List<Vector2Int> FindPath(Vector2Int startPosition, Vector2Int endPosition)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

            Node startNode = new Node(startPosition);
            Node endNode = new Node(endPosition);

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                int currentIndex = 0;

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].f < currentNode.f || (openSet[i].f == currentNode.f && openSet[i].h < currentNode.h))
                    {
                        currentNode = openSet[i];
                        currentIndex = i;
                    }
                }

                openSet.RemoveAt(currentIndex);
                closedSet.Add(currentNode.position);

                if (currentNode.position == endNode.position)
                {
                    List<Vector2Int> path = new List<Vector2Int>();
                    Node current = currentNode;
                    while (current != null)
                    {
                        path.Add(current.position);
                        current = current.parent;
                    }
                    path.Reverse();
                    return path;
                }

                foreach (Vector2Int nextPosition in GetNeighbours(currentNode.position))
                {
                    if (closedSet.Contains(nextPosition) ||
                        !_itemMovementService.CanPlaceMoveAtPosition(new Vector2Int(nextPosition.x, nextPosition.y))) 
                    {
                        continue;
                    }

                    int newCostToNeighbor = currentNode.g + 1;
                    if (newCostToNeighbor < currentNode.g || !openSet.Exists(node => node.position == nextPosition))
                    {
                        Node neighborNode = new Node(nextPosition, currentNode);
                        neighborNode.g = newCostToNeighbor;
                        neighborNode.h = Mathf.Abs(neighborNode.position.x - endNode.position.x) + Mathf.Abs(neighborNode.position.y - endNode.position.y);
                        neighborNode.f = neighborNode.g + neighborNode.h;

                        if (!openSet.Exists(node => node.position == nextPosition))
                        {
                            openSet.Add(neighborNode);
                        }
                    }
                }
            }
            return null; // No path found
        }
    }

}

