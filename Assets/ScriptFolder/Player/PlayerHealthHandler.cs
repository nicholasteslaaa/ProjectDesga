using UnityEngine;
using TMPro;

public class PlayerHealthHandler : MonoBehaviour
{

    float health = 100f;
    float oxygen = 100f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void attacked(float damage)
    {
        PlayerComponentManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>();
        health -= damage;
        player.getAnimator().Play("Attacked");
    }




    
}
