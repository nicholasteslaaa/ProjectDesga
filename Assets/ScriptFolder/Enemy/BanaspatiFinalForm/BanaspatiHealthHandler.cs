using UnityEngine;
using UnityEngine.UI;

public class BanaspatiHealthHandler : MonoBehaviour
{
    public float healthMax = 1000;
    public float health = 1000;

    public float freezeAttackedDelayTimer = -1f;
    public Slider healthSlider;


    MovementHandler movementHandler;
    Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementHandler = GetComponent<MovementHandler>();
        animator = GetComponent<Animator>();
        healthSlider.minValue = 0;
        healthSlider.maxValue = healthMax;
        
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;
        animator.SetFloat("Health",health);
        if (health <= 0)
        {
            movementHandler.triggerStop(true);
        }

        if (freezeAttackedDelayTimer < 0)
        {
            movementHandler.triggerStop(false);
        }
        else
        {
            freezeAttackedDelayTimer -= Time.deltaTime;
        }
        
    }

    public void attack(float damage,float stopTimer)
    {
        freezeAttackedDelayTimer = stopTimer;
        movementHandler.triggerStop(true);
        health -= damage;
    }
}
