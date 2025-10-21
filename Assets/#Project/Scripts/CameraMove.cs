using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public float time = 1f;
    private Vector3 positionCam;
    private Vector3 velocity;
    
    void Update()
    {
        Vector3 playerPosition = player.position;
        positionCam = new(playerPosition.x, playerPosition.y, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, positionCam, ref velocity, time); 
    }
}

