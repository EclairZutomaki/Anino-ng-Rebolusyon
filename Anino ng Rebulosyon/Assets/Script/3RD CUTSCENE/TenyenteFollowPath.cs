using UnityEngine;
using System.Collections.Generic;

public class TenyenteFollowPath : MonoBehaviour
{
    [Header("Path Settings")]
    public List<Transform> waypoints;
    public float speed = 2f;
    public float waitTime = 2f;

    [Header("Animation Settings")]
    public Animator animator;
    private readonly string animWalk = "isWalking";

    private int currentIndex = 0;
    private bool waiting = false;
    private float waitCounter = 0f;
    private bool playerInRange = false;
    private bool pathComplete = false;

    // 👇 movement tracking vars
    private Vector3 lastPosition;
    private float moveSpeed;
    private float smoothTimer = 0f;
    private bool smoothWalkingState = false;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (waypoints.Count == 0 || pathComplete)
        {
            SetWalking(false);
            return;
        }

        if (!playerInRange)
        {
            SetWalking(false);
            return;
        }

        if (waiting)
        {
            WaitAtPoint();
        }
        else
        {
            MoveToWaypoint();
        }

        // 🧠 Calculate movement speed (for smoother animation)
        moveSpeed = (transform.position - lastPosition).magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPosition = transform.position;

        bool detectedWalking = moveSpeed > 0.05f; // threshold so idle doesn’t trigger too fast

        // smooth transition between idle/walk
        if (detectedWalking)
        {
            smoothTimer += Time.deltaTime;
            if (smoothTimer > 0.15f) smoothWalkingState = true; // walking for more than 0.15s
        }
        else
        {
            smoothTimer -= Time.deltaTime;
            if (smoothTimer <= 0f) smoothWalkingState = false; // fully stopped
        }

        SetWalking(smoothWalkingState);
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentIndex];
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Rotate smoothly toward waypoint
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
            currentIndex++;

            if (currentIndex >= waypoints.Count)
            {
                pathComplete = true;
                SetWalking(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            SetWalking(false);
        }
    }

    void SetWalking(bool isWalking)
    {
        if (animator != null)
            animator.SetBool(animWalk, isWalking);
    }
}
