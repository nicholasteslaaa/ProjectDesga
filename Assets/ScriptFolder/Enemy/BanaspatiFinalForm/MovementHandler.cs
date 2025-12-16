using UnityEngine;
using UnityEngine.AI;

public class MovementHandler : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    PlayerComponentManager playerComponentManager;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;   // 👈 stop auto rotation

        playerComponentManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Distance",Vector3.Distance(playerComponentManager.transform.position,transform.position));
        agent.SetDestination(playerComponentManager.transform.position);
    }

    public void triggerStop(bool state)
    {
        if (state)
        {
            agent.velocity = Vector3.zero;
        }
        agent.isStopped = state;
    }

    public bool isStopping()
    {
        return agent.isStopped;
    }
    
}
