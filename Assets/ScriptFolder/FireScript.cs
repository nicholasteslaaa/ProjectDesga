using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour
{
    private Animator animator;

    [Header("Timers")]
    public float growTime = 30f;

    public int[] rangeSpawnSpread = { 3, 5 };

    private float growTimer;

    [Header("Spread Settings")]
    public GameObject firePrefab; // assign in Inspector

    string[] phase = { "Fire", "MedFire", "BigFire" };
    int phaseIdx = 0;

    public float damage = 5f;

    [Header("Damage Settings")]
    public float damageDelay = 3f;
    // public bool isPlayerHit = false;

    PlayerComponentManager playerComponentManager;
    NPC_Movement NPC_Script;

    void Start()
    {
        animator = GetComponent<Animator>();
        // playerComponentManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>();

        growTimer = growTime;

    }

    void Update()
    {
        // Damage
        if (playerComponentManager != null)
        {
            playerComponentManager.getPlayerHealthHandler().attacked(damage);
        }
        if (NPC_Script != null)
        {
            NPC_Script.attacked(damage);
        }
        

        // 🔥 Handle fire growth animation
        if (growTimer > 0)
        {
            growTimer -= Time.deltaTime;
        }
        else
        {
            if (phaseIdx < phase.Length - 1)
            {
                phaseIdx += 1;
                growTimer = growTime;
            }
        }

        animator.Play(phase[phaseIdx]);
    }


    public void attacked(float dmg)
    {
        if (phaseIdx <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            phaseIdx -= 1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerComponentManager = other.GetComponent<PlayerComponentManager>();
        }
        if (other.tag == "NPC")
        {
            NPC_Script = other.GetComponent<NPC_Movement>();    
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            playerComponentManager = other.GetComponent<PlayerComponentManager>();
        }
        if (other.tag == "NPC")
        {
            NPC_Script = other.GetComponent<NPC_Movement>();    
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerComponentManager.getPlayerHealthHandler().cancelAttacked();
            playerComponentManager = null;
        }
        if (other.tag == "NPC")
        {
            NPC_Script = null;
        }
    }

}
