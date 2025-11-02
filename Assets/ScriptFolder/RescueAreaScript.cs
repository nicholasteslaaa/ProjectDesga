using NUnit.Framework;
using UnityEngine;

public class RescueAreaScript : MonoBehaviour
{
    PlayerComponentManager playerComponentManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerComponentManager = other.GetComponent<PlayerComponentManager>();
        }
        if (other.tag == "NPC")
        {
            playerComponentManager.getPlayerCarryHandler().rescued(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerComponentManager = null;
        }
    }
}
