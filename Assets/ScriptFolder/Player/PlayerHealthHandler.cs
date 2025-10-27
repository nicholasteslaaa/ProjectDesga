using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerHealthHandler : MonoBehaviour
{
    public Slider healthSlider;
    public Slider oxygenSlider;


    float health = 100f;
    float oxygen = 100f;

    float delay = 0.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSlider.maxValue = 100;
        healthSlider.minValue = 0;

        oxygenSlider.maxValue = 100;
        oxygenSlider.minValue = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;
        oxygenSlider.value = oxygen;

        if (oxygen < 0)
        {
            if (delay < 0)
            {
                attacked(5);
                delay = 0.5f;
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }
        else
        {
            oxygen -= 5*Time.deltaTime;
        }
    }

    public void attacked(float damage)
    {
        PlayerComponentManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>();
        health -= damage;
        player.getAnimator().Play("Attacked");
    }




    
}
