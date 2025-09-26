using UnityEngine;
using System.Collections.Generic;

public class NPCPatrol : MonoBehaviour
{
    [Header("Path Settings")]
    [Tooltip("Waypoints the NPC will follow in order.")]
    public List<Transform> waypoints; // Assign waypoints in the Inspector

    [Tooltip("Movement speed of the NPC.")]
    public float speed = 2f;

    [Tooltip("Time the NPC waits at each waypoint before moving on.")]
    public float waitTime = 2f;

    private int currentIndex = 0;   // Current waypoint index
    private bool waiting = false;   // Is NPC waiting?
    private float waitCounter = 0f; // Timer for waiting

    void Update()
    {
        if (waypoints.Count == 0) return; // If no waypoints assigned, do nothing

        if (waiting)
        {
            WaitAtPoint();
        }
        else
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentIndex];

        // ✅ Keep NPC's current Y, only move X and Z
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Rotate NPC towards target (optional for realism)
        Vector3 direction = (targetPos - transform.position).normalized;
        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Check if reached waypoint
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            waiting = true;
        }
    }

    void WaitAtPoint()
    {
        waitCounter += Time.deltaTime;
        if (waitCounter >= waitTime)
        {
            waiting = false;
            waitCounter = 0f;
            currentIndex = (currentIndex + 1) % waypoints.Count; // Loop back to start
        }
    }
}
