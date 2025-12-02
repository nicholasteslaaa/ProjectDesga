using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GunScript : MonoBehaviour
{
    public Animator animator;
    public GameObject water;
    public GameObject smokeBomb;
    public Spawner spawner;
    public float damage = 10;

    public PlayerComponentManager playerComponentManager;

    [Header("Gun Setting")]
    public Image gunStatus;
    public int ammoAmmount = 15;
    public int ammo = 0;

    public bool isReloading = false;
    public float reloadDelay = 2f;
    public float reloadDelayTimer = 0f;

    [Header("Util 1 Setting")]
    public Image util1;
    public float cooldownTimeUtil1 = 15f;
    public float cooldownTimerUtil1 = 0;

    [Header("Power Up")]
    public float powerUpTimer = -1;
    public float damageAddition = 0;
    public int ammoAddition = 0;

    public bool ammoChangeTrigger = false;



    void Start()
    {
        ammo = ammoAmmount;
    
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"powerup; {powerUpTimer}, ammo: {ammo},ammoAdd: {ammoAddition}");
        if (powerUpTimer > 0) {
            if (!ammoChangeTrigger)
            {
                ammo = ammoAmmount+ammoAddition;
                ammoChangeTrigger = true;
            }
            powerUpTimer -= Time.deltaTime;
        }
        else {
            if (ammoChangeTrigger)
            {
                ammo = ammoAmmount;
                ammoChangeTrigger = false;
            }
            damageAddition = 0f;
            ammoAddition = 0;
        }

        if (!isReloading)
        {
            gunStatus.fillAmount = (float)ammo / ammoAmmount;
        }
        else
        {
            gunStatus.fillAmount = (reloadDelay - reloadDelayTimer)/reloadDelay;
        }
        
        util1.fillAmount = (cooldownTimeUtil1 - cooldownTimerUtil1)/cooldownTimeUtil1;
        cooldownUtil1Handle();


        Vector3 skinScale = playerComponentManager.getSkinScript().getScale();
        Vector3 newscale = water.transform.localScale;
        if (skinScale.x < 0)
        {
            newscale.x = -1;
        }
        else
        {
            newscale.x = 1;
        }

        water.transform.localScale = newscale;

        if (Input.GetMouseButtonDown(0) && ammo > 0 && reloadDelayTimer <= 0)
        {
            shoot();
        }
        if (((Input.GetKeyDown(KeyCode.R) && ammo < ammoAmmount) || ammo <= 0) && !isReloading)
        {
            setReload();
        }
        reloadHandle();



        if (Input.GetKeyDown(KeyCode.Q) && !animator.GetBool("Bombing") && playerComponentManager.getPlayerMovement().getIsGrounded() && cooldownTimerUtil1 <= 0)
        {
            // animator.SetBool("Bombing",true);
            animator.SetBool("Bombing", true);
            spawner.SpawnObject(smokeBomb);
            setCooldownUtil1();
        }
    }



    public void cooldownUtil1Handle()
    {
        if (cooldownTimerUtil1 > 0)
        {
            cooldownTimerUtil1 -= Time.deltaTime;
        }
    }

    public void setCooldownUtil1()
    {
        cooldownTimerUtil1 = cooldownTimeUtil1;
    }

    public void shoot()
    {
        WaterBulletScript waterScript = water.GetComponent<WaterBulletScript>();
        float finalDamage = damage + damageAddition;
        waterScript.setDamage(finalDamage);
        spawner.SpawnObject(water);
        ammo -= 1;
    }

    public void reloadHandle()
    {
        if (isReloading)
        {
            if (reloadDelayTimer > 0)
            {
                reloadDelayTimer -= Time.deltaTime;

            }
            else
            {
                // int finalAmmo = ammoAmmount+ammoAddition;
                isReloading = false;
                ammo = ammoAmmount;
            }
        }
    }
    
    public void setReload()
    {
        isReloading = true;
        reloadDelayTimer = reloadDelay;
    }


    public void setPowerUp(float timer,float damageAddition,int ammoAddition)
    {
        powerUpTimer = timer;
        this.damageAddition = damageAddition;
        this.ammoAddition = ammoAddition;
    }
}

