using UnityEngine;

public class NightmareAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public int damage = 1;
    public Transform player;
    private PlayerHealth playerHealth;
    public float maxTime = 3f;
    public float currentTime;
    public bool attack = false;

    void Start()
    {
       playerHealth = player.GetComponent<PlayerHealth>(); 
    }

    void Update()
    {
        if (playerHealth.IsALive)
        {
            if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                PerformAttack();
                attack = true;
            }
            
        }
    }

    void PerformAttack()
    {
        if (attack == true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= maxTime)
            {
                playerHealth.TakeDamage(damage);
                currentTime = 0;
                Debug.Log("Nightmare touche");
            }
        }
        else currentTime = 0;
    }
                
            
    
        
    
}

