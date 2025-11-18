using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[RequireComponent(typeof(PathFindingNew))]
public class NightmareMove : MonoBehaviour
{
    public Transform player; 
    public Transform Nightmare;
    private PathFindingNew pathFindingNew;        
    public Rigidbody2D rb;    
    public float chase = 4f;    
    public float stop = 5f;     
    public float speed = 2f;  
    private bool isChasing = false;  
    public float distanceWP = 0.1f;
    public float recalculateDistance = 1f;
    private int wp = 0;
    
    private Vector3 lastPlayerPos;

 
    void Start()
    {
        lastPlayerPos = player.position;
        pathFindingNew = GetComponent<PathFindingNew>();
    }

    void FixedUpdate()
    {
        Debug.Log("Cible du Nightmare: " + player.name);
        if (player == null) return;

        float distancePlayer = Vector2.Distance(transform.position, player.position);

        if (!isChasing && distancePlayer < chase) isChasing = true;
        else if (isChasing && distancePlayer > stop) isChasing = false;

        if (isChasing)
        {
            Debug.Log("Is chasing");
            // if (Vector3.Distance(lastPlayerPos, player.position) > recalculateDistance)
            // {
            //     PathFindingNew.GeneratePath();
            //     lastPlayerPos = player.position;
            // }

            if (pathFindingNew.GeneratePath())
            {
                Vector2 direction = (pathFindingNew.NextDestination - rb.position).normalized;
                Debug.DrawLine(rb.position, pathFindingNew.NextDestination, Color.cyan, 10f);
                rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
            }

            // float distanceWP = Vector2.Distance(rb.position, path.[wp]);
            // if(distanceWP < recalculateDistance)
            // {
            //     if(wp < path.Count - 1) wp ++;
            // }

        }

        
            
    }
}





   


        

 
 
    

