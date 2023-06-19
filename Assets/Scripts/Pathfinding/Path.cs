using UnityEngine;

public class Path
{
    public readonly Vector3[] lookPoints; // Array of points that define the path's waypoints
    public readonly Line[] turnBoundaries; // Array of lines that represent the turn boundaries
    public readonly int finishLineIndex; // Index of the finish line within the turn boundaries
    public readonly int slowDownIndex; // Index where the unit should start slowing down

    public Path(Vector3[] waypoints, Vector3 startPos, float turnDst, float stoppingDst)
    {
        lookPoints = waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        Vector2 previousPoint = V3ToV2(startPos);

        // Generate turn boundaries based on the waypoints
        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = V3ToV2(lookPoints[i]);
            Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDst;
            turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);
            previousPoint = turnBoundaryPoint;
        }

        float dstFromEndPoint = 0;

        // Find the index where the unit should start slowing down
        for (int i = lookPoints.Length - 1; i > 0; i--)
        {
            dstFromEndPoint += Vector3.Distance(lookPoints[i], lookPoints[i - 1]);
            if (dstFromEndPoint > stoppingDst)
            {
                slowDownIndex = i;
                break;
            }
        }
    }

    private Vector2 V3ToV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z); // Convert Vector3 to Vector2 by ignoring the y-coordinate
    }

    public void DrawWithGizmos()
    {
        Gizmos.color = Color.black;

        // Draw cubes at each look point
        foreach (Vector3 p in lookPoints)
        {
            Gizmos.DrawCube(p + Vector3.up, Vector3.one);
        }

        Gizmos.color = Color.white;

        // Draw the turn boundaries using Gizmos
        foreach (Line l in turnBoundaries)
        {
            l.DrawWithGizmos(10);
        }
    }
}