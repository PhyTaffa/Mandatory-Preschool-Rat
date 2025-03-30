using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] public bool isCuring;
    public float baseSpeed = 5f;
    [SerializeField] private float currentSpeed;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (!gameStateManager.paused && !isCuring)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement.Normalize();
        }
    }

    public Vector2 ReturnMovement()
    {
        return movement;
    }
    
    void FixedUpdate()
    {
        if (!gameStateManager.paused)
        {
            rb.velocity = Vector2.zero;
            rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
        }
    }

    
    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public float getSpeed()
    {
        return baseSpeed;
    }
    
}
