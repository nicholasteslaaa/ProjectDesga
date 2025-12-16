using NUnit.Framework;
using UnityEngine;

public class RescueAreaScript : MonoBehaviour
{
    PlayerComponentManager playerComponentManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerComponentManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            playerComponentManager.getPlayerCarryHandler().rescued(other.gameObject);
            // Destroy(other);
        }
    }

    void OnTriggerExit(Collider other)
    {

    }
}
