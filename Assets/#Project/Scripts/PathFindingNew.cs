
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;


struct TileData
{
    public int distance;
    public int step;
    public Vector3Int position;

    public TileData(Vector3Int position, int distance, int step)
    {
        this.position = position;
        this.step = step;
        this.distance = distance;
    }

    public readonly int Cost => distance + step;

    public bool Equals(TileData other)
    {
        if (position != other.position) return false;
        return true;
    }
}

public class PathFindingNew : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform player;
    Dictionary<Vector3Int, TileData> pathTilesData = new();
    List<TileData> toExplore = new();
    List<Vector3Int> allReadyCheck = new();
    List<Vector3Int> path = new();


    public void Start()
    {
        GeneratePath();
    }
    public void GeneratePath()
    {
        // clear data
        toExplore.Clear();
        path.Clear();
        allReadyCheck.Clear();
        pathTilesData.Clear();

        Vector3Int destination = tilemap.layoutGrid.WorldToCell(player.position);
        Vector3Int begin = tilemap.layoutGrid.WorldToCell(transform.position); // sa propre position  

        if (CollectData(begin, destination) == false)
        {
            Debug.LogWarning("No path found.");
        }
        else
        {
            Debug.Log("Path found :D");
        }
    }

    private bool CollectData(Vector3Int begin, Vector3Int destination)
    {
        TileData firstTileData = new TileData(begin, ManathanDistance(begin, destination), 0);
        toExplore.Add(firstTileData);

        while (toExplore.Count != 0)
        {
            TileData tile = FindLowestCost();
            toExplore.Remove(tile);

            //check arround

            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    Vector3Int positionTileArround = tile.position + new Vector3Int(x, y, 0);

                    if (!tilemap.HasTile(positionTileArround)) continue;
                    int distance = ManathanDistance(positionTileArround, destination);
                    TileData tileArround = new(positionTileArround, distance, tile.step + 1);

                    if (pathTilesData.ContainsKey(positionTileArround))
                    {
                        if (pathTilesData[positionTileArround].Cost > tileArround.Cost)
                        {
                            pathTilesData[positionTileArround] = tileArround;
                        }
                    }
                    else
                    {
                        pathTilesData.Add(positionTileArround, tileArround);
                        if (positionTileArround == destination) return true;
                    }

                    if (!allReadyCheck.Contains(positionTileArround))
                    {
                        toExplore.Add(tileArround);
                        allReadyCheck.Add(positionTileArround);
                    }
                }
            }
        }
        return false;
    }

    private TileData FindLowestCost()
    {
        TileData lowestCost = toExplore[0];

        for (int i = 1; i < toExplore.Count; i++)
        {
            if (toExplore[i].Cost < lowestCost.Cost)
            {
                lowestCost = toExplore[i];
            }
        }
        return lowestCost;
    }
    
    private int ManathanDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
    }

}
