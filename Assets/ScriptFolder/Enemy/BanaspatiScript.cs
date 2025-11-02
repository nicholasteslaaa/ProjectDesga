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
    private NavMeshAgent navMeshAgent;
    private StateMachine currentState;
    Bounds navMeshBounds;
    Vector3 destination;

    public float maxSampleDistance = 10f;
    public int maxAttempts = 30;

    [Header("Detection Settings")]
    public float detectionDistance = 8f;
    public float detectionAngle = 45f; // cone half-angle
    public int rayCount = 7; // number of rays to spread across the cone
    public LayerMask detectionLayer;

    private GameObject player;
    private GameObject lastDetectedObject; // store for gizmo visualization

    void Start()
    {
        navMeshBounds = GetNavMeshBounds();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = StateMachine.Patrol;

        destination = GetRandomPointOnNavMesh();
    }

    void Update()
    {
        GameObject objectSeen = rayCastDetect();
        if (objectSeen != null && objectSeen.CompareTag("Player"))
        {
            currentState = StateMachine.Engage;
            player = objectSeen;
        }

        if (currentState == StateMachine.Patrol)
        {
            if (Vector3.Distance(destination, transform.position) <= 3f)
            {
                destination = GetRandomPointOnNavMesh();
            }
        }
        else if (currentState == StateMachine.Engage)
        {
            destination = player.transform.position;
        }

        navMeshAgent.SetDestination(destination);
    }

    public Vector3 GetRandomPointOnNavMesh()
    {
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
        bool anyHit = false;

        // Spread rays in a cone shape
        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (rayCount - 1); // 0 to 1
            float angle = Mathf.Lerp(-detectionAngle, detectionAngle, t);
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * transform.forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, detectionDistance, detectionLayer))
            {
                Debug.DrawRay(transform.position, direction * hit.distance, Color.green);
                anyHit = true;
                lastDetectedObject = hit.collider.gameObject;

                // Return early if player detected
                if (hit.collider.CompareTag("Player"))
                    return hit.collider.gameObject;
            }
            else
            {
                Debug.DrawRay(transform.position, direction * detectionDistance, Color.red);
            }
        }

        return anyHit ? lastDetectedObject : null;
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
}
