using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public Transform item;
    public Transform candle;
    public float itemRange = 1.5f;
    public float candleRange = 1.5f;
    public float safeZone = 4f;
    public int heal = 1;
    public bool lightUp = false;
    public float maxTime = 2f;
    public float currentTime;
    public Animator animator;


    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }
    public void Update()
    {

        // if (Input.GetKey(KeyCode.E) && Vector2.Distance(transform.position, item.position) <= itemRange)
        // {
        // 
        // }

        if (Input.GetKey(KeyCode.E) && Vector2.Distance(transform.position, candle.position) <= candleRange)
        {
            lightUp = true;
            animator.SetBool("lightUp", true);
        }
        Candle();
//  Candle doit etre continuellement appeler donc pas dans le IF...
    }

    public void Candle()
    {
        if (Vector2.Distance(transform.position, candle.position) <= safeZone)
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
