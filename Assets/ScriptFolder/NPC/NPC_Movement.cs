using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
public class NPC_Movement : MonoBehaviour
{
    NavMeshAgent __agent;
    PlayerComponentManager playerComponentManager;
    Transform playerTransform;
    public GameObject interactButtonGuide;
    public Slider healthBar;
    float health = 100f;

    float delay = -1f;

    public bool isAlreadyInteract = false;

    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        __agent = GetComponent<NavMeshAgent>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerComponentManager = player.GetComponent<PlayerComponentManager>();
        playerTransform = player.transform;

        __agent.updateRotation = false;   // 👈 stop auto rotation

        interactButtonGuide.gameObject.SetActive(false);

        healthBar.minValue = 0;
        healthBar.maxValue = 100;
        
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Magnitude",__agent.velocity.magnitude);
        animator.SetFloat("Health",health);
        
        if (health <= 0) {
            __agent.velocity = Vector3.zero;
            __agent.isStopped = false;
            return;
        }

        Vector3 healthBarNewPos = transform.position;
        healthBarNewPos.y += 2;
        healthBar.transform.position = healthBarNewPos;

        healthBar.value = health;

        float playerDistance = Vector3.Distance(playerTransform.position, transform.position);
        if (playerDistance <= __agent.stoppingDistance && !isAlreadyInteract)
        {
            interactButtonGuide.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && playerComponentManager.getPlayerCarryHandler().getNumberOfFollowedNPC() < 3)
            {
                playerComponentManager.getPlayerCarryHandler().setFollowed(gameObject);
                isAlreadyInteract = true;
            }
        }
        else
        {
            interactButtonGuide.gameObject.SetActive(false);
        }

        if (isAlreadyInteract)
        {
            __agent.SetDestination(playerTransform.position);
        }
        
        if (playerDistance > __agent.stoppingDistance+5f)
        {
            playerComponentManager.getPlayerCarryHandler().unsetFollowed(gameObject);
            isAlreadyInteract = false;
        }
    }

    public void setInteract(bool status)
    {
        isAlreadyInteract = status;
    }

    public void attacked(float damage)
    {
        if (delay < 0)
        {
            if (health >= 0)
            {
                health -= damage;
            }
            delay = 0.5f;
        }
        else
        {
            delay -= Time.deltaTime;
        }
    }

    public NavMeshAgent getNavmeshagent()
    {
        return __agent;
    }

    public void setPlayerTransform(Transform pos)
    {
        playerTransform = pos;
    }

    public float getHealth()
    {
        return health;
    }
}
