using UnityEngine;

public class Items : MonoBehaviour
{
    
    public Animator animator;
    public Transform player;
    public float itemRange = 1.5f;

    public void Update()
    {

        if (Input.GetKey(KeyCode.E) && Vector2.Distance(transform.position, player.position) <= itemRange)
        {
            //en attendant de faire l'inventair
            gameObject.SetActive(false);
        }

    }

    

}
