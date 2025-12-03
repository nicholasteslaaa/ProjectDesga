using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public GameObject SpawnObject(GameObject objectToSpawn)
    {
        Vector3 position = transform ? transform.position : transform.position;
        Quaternion rotation = transform ? transform.rotation : transform.rotation;

        return Instantiate(objectToSpawn, position, rotation);
    }
}

