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
        setPinPoint();
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
    
    void setPinPoint()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            GameObject lastPinpoint = GameObject.FindGameObjectWithTag("PinPoint"); 
            if (lastPinpoint != null) {Destroy(lastPinpoint);}


            GameObject[] NPCs = GameObject.FindGameObjectsWithTag("NPC");
            // Raycast from camera through mouse cursor
            if (Physics.Raycast(ray, out hit, 1000f, groundLayer))
            {
                // Get the hit point on the floor
                Vector3 spawnPos = hit.point + Vector3.up * yOffset;
                
                foreach (GameObject NPC in NPCs){
                    NPC.GetComponent<NPC_Movement>().setPosition(spawnPos);
                }
                // Spawn the object
                Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
            }
        }
    }
}
