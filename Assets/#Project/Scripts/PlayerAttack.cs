using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private NightmareHealth nightmareHealth;
    public Transform nightmare;
    public float attackRange = 1.5f;
    public int damageAttack = 1;
    public int damageCry = 2;


    void Start()
    {
        nightmareHealth = nightmare.GetComponent<NightmareHealth>();
    }


    void Update()
    {
        if (Vector2.Distance(transform.position, nightmare.position) <= attackRange)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Attack!-1");
                PerformAttack();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Debug.Log("Cri!-2");
                PerformCry();
            }
        }
        
    }

    void PerformAttack()
    {
        nightmareHealth.TakeDamage(damageAttack);
    }

    void PerformCry()
    {
        nightmareHealth.TakeDamage(damageCry);
    }
}
