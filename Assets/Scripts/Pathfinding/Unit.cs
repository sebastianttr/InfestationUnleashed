using System.Collections;
using UnityEngine;
public class Unit : MonoBehaviour
{
    private const float minPathUpdateTime = .2f; // Minimum time interval between path updates
    private const float pathUpdateMoveThreshold = .5f; // Minimum distance threshold for triggering a path update

    public Transform target; // Target transform to move towards
    public float speed = 20; // Speed of the unit
    public float turnSpeed = 3; // Speed of turning
    public float turnDst = 5; // Distance from a waypoint at which to start turning
    public float stoppingDst = 10; // Distance from the target at which to stop
    public bool YAxisRotation = false;

    private Path path; // Current path for the unit to follow

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;

        StartCoroutine(UpdatePath()); // Start the coroutine for updating the path
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDst, stoppingDst);

            StopCoroutine("FollowPath"); // Stop the previous FollowPath coroutine
            StartCoroutine("FollowPath"); // Start a new FollowPath coroutine
        }
    }

    private IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);

            // Check if the target has moved significantly to trigger a new path request
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    private IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);

            // Check if the unit has crossed a turn boundary and update the path index accordingly
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                // Adjust speed based on the distance to the target and the stopping distance
                if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                // Rotate towards the next waypoint and move forward
                Vector3 lookDirection = path.lookPoints[pathIndex] - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            }

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos(); // Draw gizmos for the path visualization
        }
    }
}