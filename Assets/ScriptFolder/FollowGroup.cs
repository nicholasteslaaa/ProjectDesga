using UnityEngine;

public class FollowGroup : MonoBehaviour
{
    public GameObject[] targets; // Assign multiple objects in Inspector
    public float followSpeed = 5f;

    PlayerComponentManager playerComponentManager;

    void Start()
    {
        playerComponentManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>();
    }

    void Update()
    {
        targets = playerComponentManager.getPlayerCarryHandler().getNpcsFollowed();

        if (targets == null || targets.Length == 0 || playerComponentManager.getPlayerCarryHandler().getNumberOfFollowedNPC() <= 0) return;

        // Calculate group center
        Vector3 center = Vector3.zero;
        foreach (GameObject t in targets)
        {
            if (t != null)
                center += t.transform.position;
        }
        center /= targets.Length;

        // Smoothly move toward the group center
        Vector3 newpos = center;
        newpos.y = transform.position.y;   
        transform.position = Vector3.Lerp(transform.position, newpos, followSpeed * Time.deltaTime);
    }
}