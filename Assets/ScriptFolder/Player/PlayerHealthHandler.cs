using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerHealthHandler : MonoBehaviour
{
    public Slider healthSlider;
    public Slider oxygenSlider;
    public PlayerComponentManager playerComponentManager;

    float health = 100f;
    float oxygen = 100f;

    public float delay = -1f;

    public bool enabledOxygen = false;


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
        
        if (enabledOxygen)
        {
            oxygenHandler();
        }
        
    }
    
    public void oxygenHandler()
    {
        
        if (oxygen < 0)
        {
            attacked(5);
        }
        else
        {
            oxygen -= 5 * Time.deltaTime;
        }
    }

    public void attacked(float damage)
    {
        if (delay < 0)
        {
            if (health >= 0)
            {
                playerComponentManager.getAnimator().Play("Attacked");
                health -= damage;
            }
            delay = 0.5f;
        }
        else
        {
            delay -= Time.deltaTime;
        }
    }

    public void cancelAttacked()
    {
        delay = -1;
    }

    public float getHealth()
    {
        return health;
    }

    public float getOxygen()
    {
        return oxygen;
    }
    
}
