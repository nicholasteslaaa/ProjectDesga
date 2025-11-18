using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum StateMachine
{
    Patrol,
    Engage
}

[RequireComponent(typeof(NavMeshAgent))]
public class BanaspatiScript : MonoBehaviour
{
    public float healthMax = 1000f;
    public float health = 1000f;
    public Slider healthSlider;

    [Header("Fire Setting")]
    public GameObject fire;
    public float fireDelay = 1f;
    public float fireDelayTimer = -1f;

    [Header("Movement Setting")]
    public float patrolSpeed = 4f;
    public float chasingSpeed = 6f;
    public float nextDestinationDelay = 2f;
    public float nextDestinationDelayTimer = 0f;
    private NavMeshAgent navMeshAgent;
    private StateMachine currentState;
    private Vector3 destination;
    public float maxSampleDistance = 10f;
    public int maxAttempts = 30;
    [Header("1/x")]
    public int goToPlayerOnPatrolChance = 4;

    [Header("Detection Settings")]
    public float detectionDistance = 12f;
    public float detectionAngle = 45f;
    public int rayCount = 7;
    public LayerMask detectionLayer;

    private GameObject player;
    private GameObject lastDetectedObject;

    [Header("Skin")]
    public GameObject skin;
    public SpriteRenderer skinSprite;
    public Animator animator;
    public Sprite[] Phase;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = StateMachine.Patrol;

        destination = GetRandomPointOnNavMesh();

        healthSlider.minValue = 0f;
        healthSlider.maxValue = healthMax;

    }

    void Update()
    {
        healthSlider.value = health;
        skin.transform.position = transform.position;
        animator.SetFloat("Walking",navMeshAgent.velocity.magnitude);
        animator.SetFloat("Health",health);

        float cross = Vector3.Cross(transform.forward, navMeshAgent.desiredVelocity.normalized).y;

        if (cross > 0)
        {
            Debug.Log("Turn LEFT");
            Vector3 newScale = skinSprite.transform.localScale;
            newScale.x = 1;
            skinSprite.transform.localScale = newScale;
        }
        else if (cross < 0)
        {
            Debug.Log("Turn RIGHT");
            Vector3 newScale = skinSprite.transform.localScale;
            newScale.x = -1;
            skinSprite.transform.localScale = newScale;
        }
                
        
        if (nextDestinationDelayTimer > 0)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            nextDestinationDelayTimer -= Time.deltaTime;
            return;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }


        GameObject detected = rayCastDetect();

        // ✅ Detects player immediately and starts chasing
        if (detected != null && detected.CompareTag("Player"))
        {
            currentState = StateMachine.Engage;
            player = detected;
            destination = player.transform.position;
            nextDestinationDelayTimer = 0f; // CANCEL ANY DELAY
        }

        if (currentState == StateMachine.Patrol)
        {
            navMeshAgent.speed = patrolSpeed;

            if (Vector3.Distance(destination, transform.position) <= 5f)
            {
                destination = GetRandomPointOnNavMesh();
                nextDestinationDelayTimer = nextDestinationDelay;
            }
        }
        else if (currentState == StateMachine.Engage)
        {
            navMeshAgent.speed = chasingSpeed;
            destination = player.transform.position;
        }

        navMeshAgent.SetDestination(destination);

        if (isMoving(1f))
            fireSpawnHandle();
    }

    // ✅ Raycast using eye height (prevents floor blocking detection)
    public GameObject rayCastDetect()
    {
        lastDetectedObject = null;

        Vector3 rayOrigin = transform.position + transform.forward * 1.5f;

        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (rayCount - 1);
            float angle = Mathf.Lerp(-detectionAngle, detectionAngle, t);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, detectionDistance, detectionLayer))
            {
                Debug.DrawRay(rayOrigin, direction * hit.distance, Color.green);
                lastDetectedObject = hit.collider.gameObject;

                if (hit.collider.CompareTag("Player"))
                {
                    Vector3 toPlayer = (hit.collider.transform.position - rayOrigin).normalized;
                    float dist = Vector3.Distance(rayOrigin, hit.collider.transform.position);

                    if (Physics.Raycast(rayOrigin, toPlayer, out RaycastHit blockCheck, dist))
                    {
                        if (blockCheck.collider.CompareTag("Player"))
                        {
                            return hit.collider.gameObject;
                        }
                    }
                }
            }
            else
            {
                Debug.DrawRay(rayOrigin, direction * detectionDistance, Color.red);
            }
        }

        return null;
    }

    public Vector3 GetRandomPointOnNavMesh()
    {
        int chanceA = Random.Range(0, goToPlayerOnPatrolChance);
        int chanceB = Random.Range(0, goToPlayerOnPatrolChance);

        if (chanceA == chanceB)
        {
            return GameObject.FindGameObjectWithTag("Player").transform.position;
        }

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * maxSampleDistance;
            randomDirection += transform.position; // pusat di agent, bukan di center navmesh

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxSampleDistance, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        // fallback (tapi jangan diam)
        return transform.position + transform.forward * 3f;
    }

    Bounds GetNavMeshBounds()
    {
        var triangulation = NavMesh.CalculateTriangulation();
        if (triangulation.vertices.Length == 0) return new Bounds(Vector3.zero, Vector3.zero);

        Vector3 min = triangulation.vertices[0];
        Vector3 max = triangulation.vertices[0];

        foreach (Vector3 v in triangulation.vertices)
        {
            min = Vector3.Min(min, v);
            max = Vector3.Max(max, v);
        }

        return new Bounds((min + max) / 2, max - min);
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
            Vector3 newpos = transform.position + Vector3.up * 2f;
            Instantiate(fire, newpos, fire.transform.rotation);
        }
        else fireDelayTimer -= Time.deltaTime;
    }

    public void attack(float dmg, float stopDuration)
    {
        health -= dmg;
        player = GameObject.FindGameObjectWithTag("Player");
        currentState = StateMachine.Engage;
        nextDestinationDelayTimer = 0.1f;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && nextDestinationDelayTimer <= 0)
        {
            PlayerComponentManager player = other.GetComponent<PlayerComponentManager>();
            player.getPlayerHealthHandler().attacked(15);
        }
    }
}
