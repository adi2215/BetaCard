using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarPathfinding : MonoBehaviour
{
    public Tilemap tilemap; 
    public Tilemap obstacles;

    private Dictionary<Vector3Int, Node> grid = new Dictionary<Vector3Int, Node>();

    void Start()
    {
        CreateGrid();  
    }

    void CreateGrid()
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            bool isWalkable = !obstacles.HasTile(pos); 
            grid[pos] = new Node(pos, isWalkable);
        }
    }

    public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int targetPos)
    {
        if (!grid.ContainsKey(startPos) || !grid.ContainsKey(targetPos))
        {
            Debug.Log("Стартовая или целевая точка вне зоны карты.");
            return null;
        }

        Node startNode = grid[startPos];
        Node targetNode = grid[targetPos];

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>(); 
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || 
                   (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor)) continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null; 
    }

    List<Vector3Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector3Int[] directions = {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
        };

        foreach (var dir in directions)
        {
            Vector3Int neighborPos = node.position + dir;
            if (grid.ContainsKey(neighborPos))
            {
                neighbors.Add(grid[neighborPos]);
            }
        }

        return neighbors;
    }

    int GetDistance(Node a, Node b)
    {
        return Mathf.Abs(a.position.x - b.position.x) + Mathf.Abs(a.position.y - b.position.y);
    }
}
