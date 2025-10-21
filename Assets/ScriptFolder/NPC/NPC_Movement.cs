using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour
{
    NavMeshAgent __agent;
    Vector3 playerPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        __agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPosition != null){
            __agent.SetDestination(playerPosition);
        }
    }

    public void setPosition(Vector3 pos){
        playerPosition = pos;
    }
}
