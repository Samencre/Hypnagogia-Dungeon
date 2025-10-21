using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3f;
    private Vector2 movement;
    public Rigidbody2D rb;
    public PlayerHealth playerHealth;

    void Update()
    {
        if (playerHealth.IsALive)
        {
            movement.x = Input.GetAxisRaw("Horizontal"); 
            movement.y = Input.GetAxisRaw("Vertical");

            movement = movement.normalized;
            
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed; 
    }
}
