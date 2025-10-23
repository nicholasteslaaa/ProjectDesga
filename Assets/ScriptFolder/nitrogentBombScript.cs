using Unity.VisualScripting;
using UnityEngine;

public class nitrogentBombScript : MonoBehaviour
{

    public GameObject smoke;
    public float extinguishDistance = 15f;
    Rigidbody rb;
    float force = 6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(smoke, transform.position, transform.rotation);
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
