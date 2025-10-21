using System.Collections;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Timers")]
    public float invis = 1f;
    public float growTime = 30f;
    public float spreadingTime = 5f;
    public float resetTimeRange = 100f;

    private float growTimer;
    private float spreadingTimer;
    private float resetTimer;

    [Header("Spread Settings")]
    public bool isAlreadySpread = false;
    public GameObject firePrefab; // assign in Inspector

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        invis = spriteRenderer.color.a;
    
        invis = 1f;
        growTimer = growTime;
        spreadingTimer = spreadingTime;
        isAlreadySpread = false;

    }

    void Update()
    {
        // 🔥 Handle fire growth animation
        if (growTimer > 0)
        {
            growTimer -= Time.deltaTime;
            if (growTimer < (growTime / 2) && growTimer > 0)
                animator.Play("MedFire");
        }
        else
        {
            animator.Play("BigFire");
            spreadingTimer -= Time.deltaTime;
        }


        // 🌱 Spread new fire
        if (spreadingTimer < 0 && !isAlreadySpread)
        {
            isAlreadySpread = true; // prevent multiple spreads

            Vector3 newPos = new(
                transform.position.x + Random.Range(-5f, 5f),
                transform.position.y,
                transform.position.z + Random.Range(-5f, 5f)
            );

            Quaternion newRot = Quaternion.Euler(0f, -50f, 0f);

            // GameObject clone = Instantiate(firePrefab, newPos, Quaternion.identity);
            GameObject clone = Instantiate(firePrefab, newPos, newRot);
            FireScript newFire = clone.GetComponent<FireScript>();
            newFire.isAlreadySpread = false; // reset flag
            resetTimer = Random.Range(1, resetTimeRange);
        }
        

        if (isAlreadySpread)
        {
            if (resetTimer > 0 )
            {
                resetTimer -= Time.deltaTime;
            }
            if (resetTimer <= 0 )
            {
                invis = 1f;
                growTimer = (growTime/2);
                spreadingTimer = spreadingTime;
                isAlreadySpread = false;
            }
        }

        // 💨 Fade out
        Color color = spriteRenderer.color;
        color.a = invis;
        spriteRenderer.color = color;

        if (invis <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void reduceInvisible(float val)
    {
        invis -= val / 10f;
        invis = Mathf.Clamp01(invis); // avoid negative alpha
    }


}
