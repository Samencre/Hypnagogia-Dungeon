using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 8;      
    [SerializeField] private int currentHealth;
    public bool IsALive = true;
    public Transform healthbarUI;
    public GameObject hpPrefab;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealtbarUI();
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

        UpdateHealtbarUI();
    }
    
public void UpdateHealtbarUI()
    {
        foreach (Transform child in healthbarUI)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < currentHealth; i++)
        {
            Instantiate(hpPrefab, healthbarUI);
        }
    }

    public void Die()
    {
        Debug.Log("Tu es mort !");
        // Animation de mort + Game Over
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

}
