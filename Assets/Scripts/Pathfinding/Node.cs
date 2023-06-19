using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable; // Indicates if the node is walkable or blocked
    public Vector3 worldPosition; // The position of the node in the game world
    public int gridX; // The x-coordinate of the node in the grid
    public int gridY; // The y-coordinate of the node in the grid
    public int movementPenalty; // The penalty associated with traversing this node

    public int gCost; // The cost of movement from the start node to this node
    public int hCost; // The heuristic cost from this node to the target node
    public Node parent; // The parent node used for pathfinding
    private int heapIndex; // The index of the node in the heap

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost; // The total cost of movement (gCost + hCost)
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        // Compare nodes based on their fCost, and if equal, based on their hCost
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare; // Negate the result to get the desired order in the heap (smaller values first)
    }
}