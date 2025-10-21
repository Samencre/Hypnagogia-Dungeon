using UnityEngine;

public class NightmareHealth : MonoBehaviour
{
    public int maxHealth = 1;      
    [SerializeField] private int currentHealth;
    public bool IsALive = true;   

    void Awake()
    {
        currentHealth = maxHealth;
    }
    

    public void TakeDamage(int damage)
    {
        if (IsALive)
        {
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;

            if (currentHealth == 0)
            {
                IsALive = false;
                Die();
            }

        }
    }

    void Die()
    {
        Debug.Log("Nightmare detruit !");
        // Animation de mort + destruction
    }
}
