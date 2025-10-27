using UnityEngine;


public class FireSpawnerScript : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int spawnCount = 10;
    private Collider cubeCollider;

    float time = 5f;
    float timer = 0f;

    void Start()
    {
        cubeCollider = GetComponent<Collider>();
        SpawnObjectsOnTop();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            SpawnObjectsOnTop();
            timer = time;
        }
    }

    void SpawnObjectsOnTop()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = GetRandomPointOnTop();
            Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointOnTop()
    {
        Bounds bounds = cubeCollider.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        float y = bounds.max.y + 3f; // Slight offset so object isn’t clipping

        return new Vector3(x, y, z);
    }
}
