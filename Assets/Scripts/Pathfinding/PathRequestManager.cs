using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    private Queue<PathResult> results = new Queue<PathResult>(); // Queue to store the path results

    private static PathRequestManager instance; // Reference to the PathRequestManager instance
    private Pathfinding pathfinding; // Reference to the Pathfinding component

    private void Awake()
    {
        instance = this; // Set the instance to this PathRequestManager
        pathfinding = GetComponent<Pathfinding>(); // Get the Pathfinding component attached to this GameObject
    }

    private void Update()
    {
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue(); // Dequeue the next path result from the queue
                    result.callback(result.path, result.success); // Execute the callback with the path result
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathfinding.FindPath(request, instance.FinishedProcessingPath); // Start a new thread to find the path
        };
        threadStart.Invoke();
    }

    public void FinishedProcessingPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result); // Enqueue the processed path result
        }
    }
}

public struct PathResult
{
    public Vector3[] path; // Array of waypoints representing the path
    public bool success; // Flag indicating whether the path was found successfully
    public Action<Vector3[], bool> callback; // Callback function to be executed with the path result

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}

public struct PathRequest
{
    public Vector3 pathStart; // Starting position of the path
    public Vector3 pathEnd; // Target position of the path
    public Action<Vector3[], bool> callback; // Callback function to be executed with the path result

    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
}