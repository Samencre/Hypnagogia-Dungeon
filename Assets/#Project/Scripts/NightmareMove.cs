using UnityEngine;

public class NightmareMove : MonoBehaviour
{
    public Transform player;         
    public float chase = 4f;    
    public float stop = 5f;     
    public float speed = 2f;  
    public float space = 1f;
    public Rigidbody2D rb;    

    private bool isChasing = false;  

void FixedUpdate()
{
    Debug.Log("Cible du Nightmare: " + player.name);
    if (player == null) return;

    float distance = Vector2.Distance(transform.position, player.position) + space;

    if (!isChasing && distance < chase) isChasing = true;
    else if (isChasing && distance > stop) isChasing = false;

    if (isChasing)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
}
}
