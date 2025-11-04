using UnityEngine;
using UnityEngine.AI;

public enum StateMachine
{
    Patrol,
    Engage
}

[RequireComponent(typeof(NavMeshAgent))]
public class BanaspatiScript : MonoBehaviour
{
    [Header("Fire Setting")]
    public GameObject fire;
    public float fireDelay = 1f;
    public float fireDelayTimer = -1f;

    [Header("Movement Setting")]
    public float patrolSpeed = 4f;
    public float chasingSpeed = 6f;
    public float nextDestinationDelay = 2f;
    private float nextDestinationDelayTimer = 0f;
    private NavMeshAgent navMeshAgent;
    private StateMachine currentState;
    private Vector3 destination;
    public float maxSampleDistance = 10f;
    public int maxAttempts = 30;
    [Header("1/x")]
    public int goToPlayerOnPatrolChance = 4;


    [Header("Detection Settings")]
    public float detectionDistance = 8f;
    public float detectionAngle = 45f; // cone half-angle
    public int rayCount = 7; // number of rays to spread across the cone
    public LayerMask detectionLayer;

    private GameObject player;
    private GameObject lastDetectedObject; // store for gizmo visualization

    [Header("Skin")]
    public GameObject skin;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = StateMachine.Patrol;

        destination = GetRandomPointOnNavMesh();
    }

    void Update()
    {
        skin.transform.position = transform.position;
        
        GameObject objectSeen = rayCastDetect();
        if (objectSeen != null && objectSeen.CompareTag("Player"))
        {
            currentState = StateMachine.Engage;
            player = objectSeen;
        }

        if (currentState == StateMachine.Patrol)
        {
            navMeshAgent.speed = patrolSpeed;
            if (Vector3.Distance(destination, transform.position) <= 3f)
            {
                destination = GetRandomPointOnNavMesh();
                nextDestinationDelayTimer = nextDestinationDelay;
            }
        }
        else if (currentState == StateMachine.Engage)
        {

            navMeshAgent.speed = chasingSpeed;
            nextDestinationDelayTimer = 0f;
            destination = player.transform.position;
        }

        if (isMoving(1f))
        {
            fireSpawnHandle();
        }

        if (nextDestinationDelayTimer > 0)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            nextDestinationDelayTimer -= Time.deltaTime;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
        

        navMeshAgent.SetDestination(destination);

    }


    public Vector3 GetRandomPointOnNavMesh()
    {
        int chanceA = Random.Range(0, goToPlayerOnPatrolChance);
        int chanceB = Random.Range(0, goToPlayerOnPatrolChance);
        Debug.Log($"{chanceA} {chanceB}");
        if (chanceA == chanceB)
        {
            return GameObject.FindGameObjectWithTag("Player").transform.position;
        }

        Bounds navMeshBounds = GetNavMeshBounds();
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(navMeshBounds.min.x, navMeshBounds.max.x),
                navMeshBounds.center.y,
                Random.Range(navMeshBounds.min.z, navMeshBounds.max.z)
            );

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, maxSampleDistance, NavMesh.AllAreas))
                return hit.position;
        }

        Debug.LogWarning("Failed to find a valid random NavMesh position!");
        return transform.position;
    }

    Bounds GetNavMeshBounds()
    {
        var triangulation = NavMesh.CalculateTriangulation();
        if (triangulation.vertices.Length == 0)
        {
            Debug.LogWarning("No NavMesh found!");
            return new Bounds(Vector3.zero, Vector3.zero);
        }

        Vector3 min = triangulation.vertices[0];
        Vector3 max = triangulation.vertices[0];
        foreach (Vector3 v in triangulation.vertices)
        {
            min = Vector3.Min(min, v);
            max = Vector3.Max(max, v);
        }

        return new Bounds((min + max) / 2, max - min);
    }

    // --- New: Widened Raycast detection ---
    public GameObject rayCastDetect()
    {
        lastDetectedObject = null;

        // Spread rays in a cone shape
        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (rayCount - 1); // 0 to 1
            float angle = Mathf.Lerp(-detectionAngle, detectionAngle, t);
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * transform.forward;

            // Raycast checks all colliders in detectionLayer
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, detectionDistance, detectionLayer))
            {
                Debug.DrawRay(transform.position, direction * hit.distance, Color.green);

                lastDetectedObject = hit.collider.gameObject;

                // ✅ Only detect player if it's the FIRST thing the ray hits
                if (hit.collider.CompareTag("Player"))
                {
                    // Check if there's no wall between enemy and player
                    Vector3 playerDirection = (hit.collider.transform.position - transform.position).normalized;
                    float distanceToPlayer = Vector3.Distance(transform.position, hit.collider.transform.position);

                    // Raycast again, but check if the first thing hit is indeed the player
                    if (Physics.Raycast(transform.position, playerDirection, out RaycastHit blockCheck, distanceToPlayer))
                    {
                        if (blockCheck.collider.CompareTag("Player"))
                        {
                            // Player is visible — return them
                            return hit.collider.gameObject;
                        }
                        else
                        {
                            // Something blocked the view (wall, object, etc.)
                            continue;
                        }
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position, direction * detectionDistance, Color.red);
            }
        }

        return null;
    }


    // --- Draw Cone in Scene View for Debug ---
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // Draw detection range arc
        Vector3 startDir = Quaternion.Euler(0, -detectionAngle, 0) * transform.forward;
        Vector3 endDir = Quaternion.Euler(0, detectionAngle, 0) * transform.forward;

        Vector3 origin = transform.position;

        Gizmos.DrawLine(origin, origin + startDir * detectionDistance);
        Gizmos.DrawLine(origin, origin + endDir * detectionDistance);
        Gizmos.DrawWireSphere(origin + transform.forward * detectionDistance, 0.2f);

        // Optional: draw the cone edges
        for (float a = -detectionAngle; a <= detectionAngle; a += detectionAngle / 3)
        {
            Vector3 dir = Quaternion.Euler(0, a, 0) * transform.forward;
            Gizmos.DrawRay(origin, dir * detectionDistance);
        }
    }

    public bool isMoving(float threshold)
    {
        return navMeshAgent.velocity.magnitude > threshold;
    }

    public void fireSpawnHandle()
    {
        if (fireDelayTimer <= 0)
        {
            fireDelayTimer = fireDelay;
            Vector3 newpos = transform.position;
            newpos.y += 2;
            Instantiate(fire, newpos, fire.transform.rotation);
        }
        else
        {
            fireDelayTimer -= Time.deltaTime;
        }
    }
}
