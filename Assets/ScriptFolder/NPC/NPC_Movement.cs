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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        __agent = GetComponent<NavMeshAgent>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerComponentManager = player.GetComponent<PlayerComponentManager>();
        playerTransform = player.transform;

        interactButtonGuide.gameObject.SetActive(false);
        healthBar.minValue = 0;
        healthBar.maxValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) {return;}

        // set UI position
        Vector3 buttonGuideNewPos = transform.position;
        buttonGuideNewPos.y += 1.5f;
        buttonGuideNewPos.x += 1.5f;
        interactButtonGuide.transform.position = buttonGuideNewPos;

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
}
