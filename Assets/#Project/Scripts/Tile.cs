using UnityEngine;

[System.Serializable]
public class Tile
{
    public Vector2 position;
    public Vector2 connection;
    public float distanceFromStart = float.MaxValue;
    public float estimatedDistanceToEnd;
    public float totalEstimatedDistance;


    
}


