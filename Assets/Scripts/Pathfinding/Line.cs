using UnityEngine;

public struct Line
{
    private const float verticalLineGradient = 1e5f;

    private float gradient; // Gradient of the line
    private float y_intercept; // Y-intercept of the line
    private Vector2 pointOnLine_1; // First point on the line
    private Vector2 pointOnLine_2; // Second point on the line

    private float gradientPerpendicular; // Gradient of the perpendicular line

    private bool approachSide; // Indicates which side of the line to approach

    public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
    {
        // Calculate the gradient of the perpendicular line
        float dx = pointOnLine.x - pointPerpendicularToLine.x;
        float dy = pointOnLine.y - pointPerpendicularToLine.y;

        if (dx == 0)
        {
            gradientPerpendicular = verticalLineGradient; // Set a large gradient value for vertical lines
        }
        else
        {
            gradientPerpendicular = dy / dx; // Calculate the gradient using the difference in x and y coordinates
        }

        // Calculate the gradient of the line perpendicular to the original line
        if (gradientPerpendicular == 0)
        {
            gradient = verticalLineGradient; // Set a large gradient value for horizontal lines
        }
        else
        {
            gradient = -1 / gradientPerpendicular; // Calculate the negative reciprocal of the perpendicular gradient
        }

        // Calculate the y-intercept of the line
        y_intercept = pointOnLine.y - gradient * pointOnLine.x;

        // Store the points on the line
        pointOnLine_1 = pointOnLine;
        pointOnLine_2 = pointOnLine + new Vector2(1, gradient); // Generate a second point on the line using a unit vector in the x direction and the gradient

        approachSide = false;
        approachSide = GetSide(pointPerpendicularToLine); // Determine which side of the line to approach based on a given point
    }

    private bool GetSide(Vector2 p)
    {
        // Check which side of the line a given point lies on using cross product
        return (p.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (p.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    public bool HasCrossedLine(Vector2 p)
    {
        // Check if a given point has crossed the line by comparing the approach side
        return GetSide(p) != approachSide;
    }

    public float DistanceFromPoint(Vector2 p)
    {
        // Calculate the shortest distance between a given point and the line
        float yInterceptPerpendicular = p.y - gradientPerpendicular * p.x;
        float intersectX = (yInterceptPerpendicular - y_intercept) / (gradient - gradientPerpendicular);
        float intersectY = gradient * intersectX + y_intercept;
        return Vector2.Distance(p, new Vector2(intersectX, intersectY));
    }

    public void DrawWithGizmos(float length)
    {
        // Draw the line using Gizmos in the Unity Editor for debugging purposes
        Vector3 lineDir = new Vector3(1, 0, gradient).normalized; // Direction of the line
        Vector3 lineCentre = new Vector3(pointOnLine_1.x, 0, pointOnLine_1.y) + Vector3.up; // Center point of the line
        Gizmos.DrawLine(lineCentre - lineDir * length / 2f, lineCentre + lineDir * length / 2f); // Draw the line with a specified length
    }
}