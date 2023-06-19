using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid grid; // Reference to the grid component

    private void Awake()
    {
        grid = GetComponent<Grid>(); // Get the Grid component attached to this GameObject
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {

        Vector3[] waypoints = new Vector3[0]; // Array to store the calculated waypoints
        bool pathSuccess = false; // Flag indicating whether a valid path was found

        Node startNode = grid.NodeFromWorldPoint(request.pathStart); // Get the node at the starting position
        Node targetNode = grid.NodeFromWorldPoint(request.pathEnd); // Get the node at the target position
        startNode.parent = startNode; // Set the parent of the starting node to itself

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // Create a priority queue (min heap) for the open set of nodes
            HashSet<Node> closedSet = new HashSet<Node>(); // Create a hash set for the closed set of nodes
            openSet.Add(startNode); // Add the starting node to the open set

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst(); // Remove and return the node with the lowest F cost from the open set
                closedSet.Add(currentNode); // Add the current node to the closed set

                if (currentNode == targetNode)
                {
                    // Path found: print the elapsed time in milliseconds
                    //print ("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true; // Path to the target node has been found
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue; // Skip unwalkable or already evaluated nodes
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;

                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour; // Update the movement cost to the neighbour
                        neighbour.hCost = GetDistance(neighbour, targetNode); // Calculate the heuristic cost from the neighbour to the target
                        neighbour.parent = currentNode; // Update the parent of the neighbour

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour); // Add the neighbour to the open set
                        else
                            openSet.UpdateItem(neighbour); // Update the neighbour's position in the open set
                    }
                }
            }
        }

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode); // Retrace the path from start to target node
            pathSuccess = waypoints.Length > 0; // Check if waypoints were generated
        }

        callback(new PathResult(waypoints, pathSuccess, request.callback)); // Execute the callback with the path result
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>(); // List to store the nodes along the path
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent; // Traverse from end to start by following the parent nodes
        }

        Vector3[] waypoints = SimplifyPath(path); // Simplify the path by removing redundant waypoints
        Array.Reverse(waypoints); // Reverse the order of waypoints to start from the beginning
        return waypoints;
    }

    private Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>(); // List to store the simplified waypoints
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);

            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition); // Add a waypoint if the direction changes
            }

            directionOld = directionNew;
        }

        return waypoints.ToArray(); // Convert the list of waypoints to an array
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY); // Diagonal cost: 14, Straight cost: 10
        return 14 * dstX + 10 * (dstY - dstX); // Diagonal cost: 14, Straight cost: 10
    }
}