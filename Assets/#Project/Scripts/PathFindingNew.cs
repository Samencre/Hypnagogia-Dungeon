
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using System.Text.RegularExpressions;


struct TileData
{
    public float distance;
    public int step;
    public Vector3Int position;
    public Vector3Int from;

    public TileData(Vector3Int position, Vector3Int from, float distance, int step)
    {
        this.position = position;
        this.from = from;
        this.step = step;
        this.distance = distance;
    }

    public readonly float Cost => distance + step;

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
    public List<Vector3Int> path = new();

    [SerializeField] bool debug;


    public void Start()
    {
        GeneratePath();
    }

    public bool GeneratePath()
    {
        // clear data
        toExplore.Clear();
        path.Clear();
        allReadyCheck.Clear();
        pathTilesData.Clear();

        Vector3Int destination = tilemap.layoutGrid.WorldToCell(player.position);
        Vector3Int begin = tilemap.layoutGrid.WorldToCell(transform.position); // sa propre position  

        if (CollectData(begin, destination))
        {
            Debug.Log("Path found :D");
            CreatePath(begin, destination);
            Debug.Log($"Path count: {path.Count}");
            return true;

        }
   
        Debug.LogWarning("No path found.");
        return false;
        
    }

    private void CreatePath(Vector3Int begin, Vector3Int destination)
    {

        Vector3Int currentPosition = destination;
        
        while(currentPosition != begin)
        {
        path.Add(currentPosition);
        currentPosition = pathTilesData[currentPosition].from;     
        }
        path.Reverse();

    }

    private bool CollectData(Vector3Int begin, Vector3Int destination)
    {
        TileData firstTileData = new TileData(begin, begin, Vector3Int.Distance(begin, destination), 0);
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
                    float distance = Vector3Int.Distance(positionTileArround, destination);
                    TileData tileArround = new(positionTileArround, tile.position, distance, tile.step + 1);

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

    public Vector2 NextDestination => tilemap.CellToWorld(path[0]);
    
}
