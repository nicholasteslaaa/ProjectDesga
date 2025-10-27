using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public PlayerComponentManager playerComponentManager;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerComponentManager.getAnimator().SetBool("Shooting", true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            playerComponentManager.getAnimator().SetBool("Shooting", false);
        }

        // Check if player is grounded
        isGrounded = controller.isGrounded;
        playerComponentManager.getAnimator().SetBool("Jumping", !isGrounded);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // Keeps grounded properly

        // Get input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move relative to current facing direction
        // Vector3 move = transform.right * x + transform.forward * z;
        Vector3 move = Vector3.right * x + Vector3.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        // Set the Animator float parameter
        playerComponentManager.getAnimator().SetFloat("Velocity", move.magnitude);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        float finalGravity = gravity;
        if (!isGrounded)
        {
            finalGravity -= 5;
        }


        // Apply gravity
        // velocity.y += gravity * Time.deltaTime;
        velocity.y += finalGravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public bool getIsGrounded()
    {
        return controller.isGrounded;
    }



    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float pushForce = 10f;
        Rigidbody rb = hit.collider.attachedRigidbody;

        // Skip if no rigidbody or is kinematic
        if (rb == null || rb.isKinematic) return;

        // Ignore if we are hitting the object from below (avoid pushing objects you stand on)
        if (hit.moveDirection.y < -0.3f) return;

        // Compute push direction (horizontal only)
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
    }

}
