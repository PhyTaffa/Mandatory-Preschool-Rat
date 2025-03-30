using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] public bool isCuring;

    public float baseSpeed = 5f;
    [SerializeField] private float currentSpeed;

    private Rigidbody2D rb;
    private Vector2 movement;
    
    // (Optional) Cache the Animation component to avoid repeated GetComponent<> calls
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;

        animator = GetComponent<Animator>(); // For convenience
    }

    void Update()
    {
        // Only move if the game is not paused and not "curing"
        if (!gameStateManager.paused && !isCuring)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Normalize so diagonal movement won't be faster
            movement.Normalize();
        }
        else
        {
            // If paused or curing, no movement
            movement = Vector2.zero;
        }

        // If there's no movement, set Idle = true, everything else = false
        if (movement.magnitude <= 0.01f)
        {
            // Idle true
            animator.SetBool("ItsIdle", true);
            // Walk/Run directions false
            animator.SetBool("ItsWalkTop", false);
            animator.SetBool("ItsWalkBott", false);
            animator.SetBool("ItsWalkLeft", false);
            animator.SetBool("ItsWalkRight", false);

            return; // Done for this frame
        }
        
        // We have some movement, so Idle is false
        animator.SetBool("ItsIdle", false);

        float absX = Mathf.Abs(movement.x);
        float absY = Mathf.Abs(movement.y);

        if (absX > absY)
        {
            // Horizontal movement is dominant
            if (movement.x > 0)
            {
                // Right
                animator.SetBool("ItsWalkRight", true);
                animator.SetBool("ItsWalkLeft", false);
                animator.SetBool("ItsWalkTop", false);
                animator.SetBool("ItsWalkBott", false);
            }
            else
            {
                // Left
                animator.SetBool("ItsWalkRight", false);
                animator.SetBool("ItsWalkLeft", true);
                animator.SetBool("ItsWalkTop", false);
                animator.SetBool("ItsWalkBott", false);
            }
        }
        else
        {
            // Vertical movement is dominant
            if (movement.y > 0)
            {
                // Up
                animator.SetBool("ItsWalkTop", true);
                animator.SetBool("ItsWalkBott", false);
                animator.SetBool("ItsWalkLeft", false);
                animator.SetBool("ItsWalkRight", false);
            }
            else
            {
                // Down
                animator.SetBool("ItsWalkTop", false);
                animator.SetBool("ItsWalkBott", true);
                animator.SetBool("ItsWalkLeft", false);
                animator.SetBool("ItsWalkRight", false);
            }
        }
    }

    void FixedUpdate()
    {
        if (!gameStateManager.paused)
        {
            // Clear velocity so there's no "drift"
            rb.velocity = Vector2.zero;
            
            // Move using MovePosition for smoother physics
            rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
        }
    }

    // Returns the current movement vector
    public Vector2 ReturnMovement()
    {
        return movement;
    }

    // Allows changing speed at runtime
    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    // Returns the base speed (not the current speed)
    public float getSpeed()
    {
        return baseSpeed;
    }
}
