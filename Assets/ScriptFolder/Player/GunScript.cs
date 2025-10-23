using UnityEngine;

public class GunScript : MonoBehaviour
{
    public SkinScript skinScript;
    public Animator animator;
    public GameObject water;
    public GameObject smokeBomb;
    
    public Spawner spawner;
    public float damage = 10;

    // Update is called once per frame
    void Update()
    {
        Vector3 skinScale = skinScript.getScale();
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Bombing");
            spawner.SpawnObject(smokeBomb);
        }
    }
}
