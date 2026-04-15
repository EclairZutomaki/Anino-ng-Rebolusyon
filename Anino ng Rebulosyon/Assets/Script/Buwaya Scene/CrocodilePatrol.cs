using UnityEngine;
using System.Collections.Generic;

public class CrocodilePatrol : MonoBehaviour
{
    [Header("Path Settings")]
    public List<Transform> waypoints;
    public float speed = 2f;
    public float waitTime = 2f;

    [Header("Reset Settings")]
    public Transform resetPoint; // Where crocodile teleports after hitting player
    public string playerTag = "Player";

    private int currentIndex = 0;
    private bool waiting = false;
    private float waitCounter = 0f;

    void Update()
    {
        if (waypoints.Count == 0) return;

        if (waiting)
            WaitAtPoint();
        else
            MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentIndex];

        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Rotate toward movement
        Vector3 direction = (targetPos - transform.position).normalized;
        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

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
            currentIndex = (currentIndex + 1) % waypoints.Count;
        }
    }

    // 🔥 COLLISION LOGIC
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        ResetCrocodile();
    }

    void ResetCrocodile()
    {
        if (resetPoint == null)
        {
            Debug.LogWarning("Reset point not assigned!");
            return;
        }

        // Teleport to reset position
        transform.position = resetPoint.position;

        // Reset movement state
        currentIndex = 0;
        waiting = false;
        waitCounter = 0f;
    }
}