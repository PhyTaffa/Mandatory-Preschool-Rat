using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float baseSpeed = 5f;
    private float currentSpeed;
    private Rigidbody2D rb;
    private Vector2 movement;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
    }
    
    void FixedUpdate()
    {
        rb.velocity = Vector2.zero; 
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    
    public void SetSpeed(int speed)
    {
        currentSpeed = speed;
    }

    public float getSpeed()
    {
        return currentSpeed;
    }
    
}
