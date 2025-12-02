using UnityEngine;

public class HolyWaterScript : MonoBehaviour
{
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
            PlayerComponentManager playerComponent = other.GetComponent<PlayerComponentManager>();
            if (playerComponent.getGunScript().powerUpTimer > 0) {   
                return;    
            }
            playerComponent.getGunScript().setPowerUp(10f,1000f,1000);
            Destroy(gameObject);
        }
    }
}
