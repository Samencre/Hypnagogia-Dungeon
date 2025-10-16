using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;      
    public int currentHealth;     

    void Start() // ou Awake??
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log("Vie joueur = " + currentHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Tu es mort !");
        // Game Over
    }

    public void Heal(int damage)
    {
        currentHealth += damage;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        Debug.Log("Vie joueur = " + currentHealth);
    }

}
