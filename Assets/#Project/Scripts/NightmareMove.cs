using UnityEngine;

public class NightmareMove : MonoBehaviour
{
    public Transform player;         
    public float chase = 4f;    
    public float stop = 5f;     
    public float speed = 2f;     

    private bool isChasing = false;  

    void Update()
    {
        if (player == null) return;

        // Calcul la distance entre le Nightmare et le player
        float distance = Vector2.Distance(transform.position, player.position);

        if (!isChasing && distance < chase)
        {
            isChasing = true; 
        }
        else if (isChasing && distance > stop)
        {
            isChasing = false; 
        }

        if (isChasing)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }
}
