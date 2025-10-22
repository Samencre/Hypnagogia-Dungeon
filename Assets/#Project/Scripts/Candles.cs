using UnityEngine;

public class Candles : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public Transform player;
    public Animator animator;
    public float candleRange = 1.5f;
    public float safeZone = 4f;
    public int heal = 1;
    public bool lightUp = false;
    public float maxTime = 2f;
    public float currentTime;

    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    public void Update()
    {

        if (Input.GetKey(KeyCode.E) && Vector2.Distance(transform.position, player.position) <= candleRange)
        {
            lightUp = true;
            animator.SetBool("lightUp", true);
        }
        Candle();

    }
    
    public void Candle()
    {
        if (Vector2.Distance(transform.position, player.position) <= safeZone)
        {
            if (lightUp)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= maxTime)
                {
                    playerHealth.Heal(heal);
                    currentTime = 0;
                }
            }
        }
        else currentTime = 0;
    }
}
