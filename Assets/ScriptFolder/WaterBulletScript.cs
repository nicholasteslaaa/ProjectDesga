using UnityEngine;

public class WaterBulletScript : MonoBehaviour
{
    public float speed = 5f;
    public float timeToDestroy = 5f;

    float SplashTimer = 0.1f;
    bool hit = false;

    float damage = 5;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!hit)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            timeToDestroy -= Time.deltaTime;
        }

        if (timeToDestroy < 0 || hit)
        {
            animator.Play("Splash");
            SplashTimer -= Time.deltaTime;
            if (SplashTimer < 0) { Destroy(gameObject); }
        }
    }
    
    public void setDamage(float val)
    {
        damage = val;
    }

    void OnTriggerEnter(Collider other)
    {
        FireScript fire = other.GetComponent<FireScript>();
        if (fire)
        {
            fire.attacked(damage);
        }
        hit = true;

        Rigidbody rb = other.attachedRigidbody;
        float pushForce = 10f;
        if (rb != null)
        {
            Vector3 pushDir = (other.transform.position - transform.position).normalized;
            rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }

}
