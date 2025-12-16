    using UnityEngine;

public class AttackScriptBanaspati : StateMachineBehaviour
{
    public GameObject prefab;
    public int fireCount = 8;
    public float fireRadius = 3f;

    float stoppingDelay = 0.5f;
    float stoppingDelayTimer = -1f;
    
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Spawn(animator.transform);

        stoppingDelayTimer = stoppingDelay;
    }
 
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (stoppingDelayTimer <= 0)
        {
            animator.GetComponent<MovementHandler>().triggerStop(true);
        }
        else
        {
            stoppingDelayTimer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire");
        if (fires.Length >= 100)
        {
            for(int i = 0; i < fireCount; i++)
            {
                Destroy(fires[i]);
            }
        }

        Spawn(animator.transform,fireCount,fireRadius);
        animator.GetComponent<MovementHandler>().triggerStop(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    public void Spawn(Transform transform, int count, float radius)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            Vector3 offset = new Vector3(
                Mathf.Cos(angle),
                0.3f,
                Mathf.Sin(angle)
            ) * radius;

            Instantiate(prefab, transform.position + offset, prefab.transform.rotation);
        }
    }
}
