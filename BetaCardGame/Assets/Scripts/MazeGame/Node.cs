using UnityEngine;

public class Node
{
    public Vector3Int position;  
    public bool isWalkable;  
    
    public Node parent;
    public int gCost, hCost;  
    
    public int fCost => gCost + hCost;  

    public Node(Vector3Int pos, bool walkable)
    {
        position = pos;
        isWalkable = walkable;
    }
}
