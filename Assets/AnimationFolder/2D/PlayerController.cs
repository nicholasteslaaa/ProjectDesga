using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float move = 0f;

        // Left / Right Movement using GetKey
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            move = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            move = 1f;

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Flip sprite direction
        if (move > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

}
