using UnityEngine;
using System.Collections.Generic;

public class TenyenteFollowPath : MonoBehaviour
{
    [Header("Path Settings")]
    [Tooltip("Waypoints the NPC will follow in order.")]
    public List<Transform> waypoints; // Assign waypoints in Inspector

    [Tooltip("Movement speed of the NPC.")]
    public float speed = 2f;

    [Tooltip("Time the NPC waits at each waypoint before moving on.")]
    public float waitTime = 2f;

    private int currentIndex = 0;
    private bool waiting = false;
    private float waitCounter = 0f;
    private bool playerInRange = false;
    private bool pathComplete = false;

    void Update()
    {
        // Stop everything if path finished or no waypoints
        if (waypoints.Count == 0 || pathComplete) return;

        // Move only if player is inside collider
        if (!playerInRange) return;

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
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        // Move towards target
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Rotate smoothly
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

            // Move to next waypoint (no looping)
            currentIndex++;

            // ✅ If Tenyente has reached the final waypoint, stop moving
            if (currentIndex >= waypoints.Count)
            {
                pathComplete = true;
            }
        }
    }

    // 🧠 Detect player entering/leaving trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
