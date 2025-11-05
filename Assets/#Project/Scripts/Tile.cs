using UnityEngine;

[System.Serializable]
public class Tile
{
    public Vector2 position;
    // Position du centre de la tuile
    public Vector2 connection;
    // D'où on vient pour arriver ici
    public float distanceFromStart = float.MaxValue;
    // Distance réelle depuis le départ
    public float estimatedDistanceToEnd;
    // Distance estimée jusqu’à la fin
    public float totalEstimatedDistance;
    // Somme des deux → pour comparaison

    
}


