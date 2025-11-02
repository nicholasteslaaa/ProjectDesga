using UnityEngine;
using TMPro;
using System;

public class MouseScript : MonoBehaviour
{
    public float rotationSpeed = 10f;  // smooth rotation speed
    public LayerMask groundLayer;      // assign your "Ground" layer in Inspector

    public GameObject objectToSpawn;  // drag your prefab here in the Inspector
    public float yOffset = 0.01f;     // slightly raise spawn to avoid clipping
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseLook();
    }

    void HandleMouseLook()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;


        // Cast the ray to find where the mouse points on the ground
        if (Physics.Raycast(ray, out hit, 100f, groundLayer))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = transform.position.y; // ignore height difference

            // Direction from player to target
            Vector3 direction = targetPos - transform.position;

            // Only rotate if there's actually some distance
            if (direction.sqrMagnitude > 0.01f)
            {
                // Target rotation
                Quaternion targetRotation = Quaternion.LookRotation(direction);


                // Smoothly rotate toward target
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}
