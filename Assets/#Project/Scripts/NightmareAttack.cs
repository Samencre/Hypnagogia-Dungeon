using UnityEngine;

public class NightmareAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public int damage = 1;
    public Transform player;
    private PlayerHealth playerHealth;

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
            }
            
        }
    }

    void PerformAttack()
    {
        playerHealth.TakeDamage(damage);
    }
}

