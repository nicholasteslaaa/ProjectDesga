using UnityEngine;
using FirstGearGames.SmoothCameraShaker;
public class nitrogentBombScript : MonoBehaviour
{

    public ShakeData shakeData;
    public GameObject smoke;
    public float extinguishDistance = 15f;
    Rigidbody rb;
    float force = 6;

    public AudioClip boomSoundClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {

        PlayerComponentManager playerComponentManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>();
        playerComponentManager.getAudioSource().PlayOneShot(boomSoundClip); // play boom sound
        playerComponentManager.getAnimator().SetBool("Bombing", false);

        
        CameraShakerHandler.Shake(shakeData);

        Instantiate(smoke, transform.position, transform.rotation);

        BanaspatiScript banaspati = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BanaspatiScript>();
        float enemyDistance = Vector3.Distance(transform.position, banaspati.transform.position);
        if ( enemyDistance <= extinguishDistance)
        {
            banaspati.attack(extinguishDistance,(extinguishDistance-enemyDistance)/2);
        }

        
        GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire");
        foreach (GameObject fire in fires)
        {
            float distance = Vector3.Distance(transform.position, fire.transform.position);
            if (distance <= extinguishDistance)
            {
                Destroy(fire);
            }
        }
        Destroy(gameObject);
    }
}
