using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Animator animator;
    public GameObject water;
    public GameObject smokeBomb;
    public Spawner spawner;
    public float damage = 10;

    public PlayerComponentManager playerComponentManager;   

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetMouseButtonDown(0))
        {
            WaterBulletScript waterScript = water.GetComponent<WaterBulletScript>();
            waterScript.setDamage(damage);
            spawner.SpawnObject(water);
        }
        if (Input.GetKeyDown(KeyCode.Q) && !animator.GetBool("Bombing") && playerComponentManager.getPlayerMovement().getIsGrounded())
        {
            // animator.SetBool("Bombing",true);
            animator.SetBool("Bombing",true);
            spawner.SpawnObject(smokeBomb);
        }
    }
}
