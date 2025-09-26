using UnityEngine;
using UnityEngine.AI;

public class GuardiaCivilPatrolChase : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Player transform to chase.")]
    public Transform player;

    [Tooltip("Center of the restricted area (circle).")]
    public Transform restrictedCenter;

    [Tooltip("If empty, will be created at guard's start position.")]
    public Transform guardPost;

    [Tooltip("Waypoints for patrol. Guard will ping-pong (back and forth).")]
    public Transform[] patrolPoints;

    [Header("Restricted Area")]
    [Tooltip("Radius of the restricted area in meters.")]
    public float restrictedRadius = 8f;

    [Header("Movement Speeds")]
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 3.5f;
    public float returnSpeed = 2.5f;

    [Header("Patrol Settings")]
    [Tooltip("How close to a waypoint to count as 'arrived'.")]
    public float waypointTolerance = 0.3f;

    [Tooltip("Pause time at each waypoint.")]
    public float waitAtWaypoint = 1.0f;

    [Tooltip("How often the chase destination updates (saves CPU).")]
    public float chaseRefresh = 0.1f;

    private NavMeshAgent agent;
    private int patrolIndex = 0;
    private bool patrolForward = true;
    private float waitTimer = 0f;
    private float chaseTimer = 0f;

    private enum State { Patrol, Chase, Return }
    private State state = State.Patrol;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!agent) Debug.LogError("NavMeshAgent missing on " + name);
    }

    void Start()
    {
        if (guardPost == null)
        {
            var post = new GameObject(name + "_Post");
            post.transform.position = transform.position;
            guardPost = post.transform;
        }

        // Smooth patrol: agent autoBraking off is nice for waypoint movement
        agent.autoBraking = false;
        agent.stoppingDistance = 0f;

        // If we have patrol points, go to the first. Else sit at post.
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            agent.speed = patrolSpeed;
            GoToPatrolPoint(patrolIndex);
        }
        else
        {
            agent.speed = patrolSpeed;
            agent.SetDestination(guardPost.position);
        }
    }

    void Update()
    {
        if (player == null || restrictedCenter == null || agent == null) return;

        bool playerInsideRestricted = IsPlayerInsideRestricted();

        switch (state)
        {
            case State.Patrol:
                if (playerInsideRestricted)
                {
                    StartChase();
                }
                else
                {
                    PatrolUpdate();
                }
                break;

            case State.Chase:
                if (!playerInsideRestricted)
                {
                    StartReturn();
                }
                else
                {
                    // Update chase target at intervals, not every frame
                    chaseTimer -= Time.deltaTime;
                    if (chaseTimer <= 0f)
                    {
                        chaseTimer = chaseRefresh;
                        agent.SetDestination(player.position);
                    }
                }
                break;

            case State.Return:
                if (playerInsideRestricted)
                {
                    StartChase();
                }
                else
                {
                    if (!agent.pathPending && agent.remainingDistance <= waypointTolerance)
                    {
                        // Back at post → resume patrol
                        state = State.Patrol;
                        agent.speed = patrolSpeed;

                        if (patrolPoints != null && patrolPoints.Length > 0)
                        {
                            // jump to nearest waypoint to keep it natural
                            patrolIndex = GetNearestPatrolIndex(transform.position);
                            // make the next leg go away from us (more natural ping-pong)
                            patrolForward = GetForwardDirectionFromIndex(patrolIndex);
                            GoToPatrolPoint(patrolIndex);
                        }
                        else
                        {
                            agent.ResetPath();
                        }
                    }
                }
                break;
        }
    }

    // ---------- State helpers ----------
    void StartChase()
    {
        state = State.Chase;
        agent.speed = chaseSpeed;
        chaseTimer = 0f; // force immediate destination set
        agent.SetDestination(player.position);
    }

    void StartReturn()
    {
        state = State.Return;
        agent.speed = returnSpeed;
        agent.SetDestination(guardPost.position);
    }

    // ---------- Patrol logic ----------
    void PatrolUpdate()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            // Idle at post
            if (!agent.pathPending && agent.remainingDistance <= waypointTolerance)
            {
                agent.ResetPath();
            }
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= waypointTolerance)
        {
            // Arrived → wait → then move to next
            if (waitAtWaypoint > 0f)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer < waitAtWaypoint) return;
            }

            waitTimer = 0f;
            patrolIndex = NextPatrolIndex(patrolIndex);
            GoToPatrolPoint(patrolIndex);
        }
    }

    void GoToPatrolPoint(int index)
    {
        if (index >= 0 && index < patrolPoints.Length)
        {
            agent.SetDestination(patrolPoints[index].position);
        }
    }

    int NextPatrolIndex(int current)
    {
        if (patrolPoints.Length < 2) return current;

        if (patrolForward)
        {
            if (current + 1 >= patrolPoints.Length)
            {
                patrolForward = false;
                return current - 1; // bounce back
            }
            return current + 1;
        }
        else
        {
            if (current - 1 < 0)
            {
                patrolForward = true;
                return current + 1; // bounce forward
            }
            return current - 1;
        }
    }

    int GetNearestPatrolIndex(Vector3 fromPos)
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return 0;
        int best = 0;
        float bestDist = Mathf.Infinity;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float d = (patrolPoints[i].position - fromPos).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = i;
            }
        }
        return best;
    }

    bool GetForwardDirectionFromIndex(int index)
    {
        // If on an end, make the next move go inward.
        if (index <= 0) return true; // go forward
        if (index >= patrolPoints.Length - 1) return false; // go backward
        // Middle: keep current direction
        return patrolForward;
    }

    // ---------- Utils ----------
    bool IsPlayerInsideRestricted()
    {
        float r2 = restrictedRadius * restrictedRadius;
        return (player.position - restrictedCenter.position).sqrMagnitude <= r2;
    }

    void OnDrawGizmosSelected()
    {
        // Restricted area
        if (restrictedCenter != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
            Gizmos.DrawSphere(restrictedCenter.position, restrictedRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(restrictedCenter.position, restrictedRadius);
        }

        // Patrol path
        if (patrolPoints != null && patrolPoints.Length > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < patrolPoints.Length - 1; i++)
            {
                if (patrolPoints[i] && patrolPoints[i + 1])
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
            }
        }

        // Guard post
        if (guardPost != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(guardPost.position + Vector3.up * 0.5f, new Vector3(0.4f, 1f, 0.4f));
        }
    }
}
