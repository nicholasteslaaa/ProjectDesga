using UnityEngine;

public class LookDirectionHandle : MonoBehaviour
{
    Vector3 originalScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 direction = (playerPos - transform.position).normalized;

        // Compare against A's right direction
        float side = Vector3.Dot(direction, transform.right);

        if (side > 0)
        {
            Debug.Log($"Player is on the RIGHT side of Banaspati {direction}");
            transform.localScale = directionChanger(new float[] {-originalScale.x,transform.localScale.y,transform.localScale.z});
        }
        else if (side < 0)
        {
            Debug.Log($"Player is on the LEFT side of Banaspati {direction}");
            transform.localScale = directionChanger(new float[] {originalScale.x,transform.localScale.y,transform.localScale.z});
        }
    }

    Vector3 directionChanger(float[] dir)
    {
        Vector3 newScale = transform.localScale;
        newScale.x = dir[0];
        newScale.y = dir[1];
        newScale.z = dir[2];
        
        return newScale;
    }
}
