using UnityEngine;

public class FacingTarget : MonoBehaviour
{
    public float __rotationalSpeed = 5f;
    private Transform __target;

    void Start()
    {
        __target = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        handleLook();
    }

    void handleLook()
    {
        if (__target == null) return;

        // Direction from CCTV joint to target
        Vector3 direction = __target.position - transform.position;

        // Ignore vertical difference so it doesn't look up or down
        direction.y = 0f;

        // If direction vector becomes zero (same position), skip rotation
        if (direction.sqrMagnitude < 0.0001f) return;

        // Step 1: Get horizontal look rotation
        Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);

        // Step 2: Smooth rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, __rotationalSpeed * Time.deltaTime);
    }
}
