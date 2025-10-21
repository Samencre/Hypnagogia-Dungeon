using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public int damageAttack = 1;
    public int damageCry = 2;

    public Transform nightmare;
    private NightmareHealth nightmareHealth;

    void Start()
    {
        nightmareHealth = nightmare.GetComponent<NightmareHealth>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Attack!");
            PerformAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Cri!");
            PerformCry();
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
