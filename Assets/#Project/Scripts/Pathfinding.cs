using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private int tileWidth = 1;
    [SerializeField] private int tileHeight = 1;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private bool newPath;
    [SerializeField] private bool pathGenerated;
    private Dictionary<Vector2, Tile> tiles;
    private List<Vector2> tilesToSearch;
    private List<Vector2> searchedTiles;
    private List<Vector2> finalPath;
    public Transform nightmare;
    public Transform player;
    public Transform ground;

    public float chase = 4f;    
    public float stop = 5f;     
    public float speed = 2f;  
    public Rigidbody2D rb;
    private bool isChasing = false;
    [SerializeField] private bool visualiseGrid;
    [SerializeField] private bool showTexts;

    [SerializeField] private Transform textPrefab;
    [SerializeField] private Transform textParent;


    void Awake()
    {
        gridWidth = groundTilemap.size.x;
        gridHeight = groundTilemap.size.y;
    }

    private void Update()

    {

        Debug.Log("Cible du Nightmare: " + player.name);
        // Debug.Log($"player position: {player.transform.position}");
        // Debug.Log($"map size: {groundTilemap.size}");
        // Debug.Log($"cell size: {groundTilemap.cellSize}");

        // Debug.Log($"player cell: {groundTilemap.layoutGrid.WorldToCell(player.transform.position)}");

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (!isChasing && distance < chase)
        {
            isChasing = true;
            newPath = true;
        }
        else if (isChasing && distance > stop)
        {
            isChasing = false;
            newPath = false; 
        }

        if (isChasing)
        {
            Debug.Log("Chassing");

            if (newPath && !pathGenerated)
            {
                Debug.Log("NewPath");
                GenerateGrid();
                Vector3Int playerGridPos = groundTilemap.layoutGrid.WorldToCell(player.position) - groundTilemap.origin;
                Vector3Int nightmareGridPos = groundTilemap.layoutGrid.WorldToCell(nightmare.position) - groundTilemap.origin;


                FindPath(new Vector2(nightmareGridPos.x, nightmareGridPos.y),
                         new Vector2(playerGridPos.x, playerGridPos.y));

                newPath = false;
                pathGenerated = true;

                 if (showTexts)
                {
                    VisualiseText();
                }

            }
            else if (!newPath)
            {
                pathGenerated = false;
            }
            
        }


    }


    private void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (groundTilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    Vector2 gridPos = new Vector2(x, y);
                    Tile tile = new Tile();
                    tile.position = gridPos;
                    tiles.Add(gridPos, tile);
                }
            }
        }

        // Debug.Log($"Taille max: {gridWidth * gridHeight} | Tiles générées: {tiles.Count}");
 
    }
 

    private void FindPath(Vector2 startPos, Vector2 endPos)
    {
        Debug.Log("FindPath");
        tilesToSearch = new List<Vector2> { startPos };
        searchedTiles = new List<Vector2>();
        finalPath = new List<Vector2>();
 
        
        tiles[startPos].distanceFromStart = 0;
        tiles[startPos].estimatedDistanceToEnd = Vector2.Distance(startPos, endPos);
        tiles[startPos].totalEstimatedDistance = Vector2.Distance(startPos, endPos);
           
 
        
        while (tilesToSearch.Count > 0)
        {
           
            Vector2 tileToSearch = tilesToSearch[0];
            foreach (Vector2 pos in tilesToSearch)
            {
                Tile t = tiles[pos];
                if (t.totalEstimatedDistance < tiles[tileToSearch].totalEstimatedDistance ||
                    (t.totalEstimatedDistance == tiles[tileToSearch].totalEstimatedDistance &&
                     t.estimatedDistanceToEnd < tiles[tileToSearch].estimatedDistanceToEnd))
                {
                    tileToSearch = pos;
                }
            }
 
            tilesToSearch.Remove(tileToSearch);
            searchedTiles.Add(tileToSearch);
 
            if (tileToSearch == endPos)
            {
                Tile pathTile = tiles[endPos];

                while (pathTile.position != startPos)
                {
                    finalPath.Add(pathTile.position);
                    pathTile = tiles[pathTile.connection];
                    
                }

                finalPath.Add(startPos);
                VisualiseText();
 
                Debug.Log($"Chemin trouvé ! Longueur : {finalPath.Count}");

                return;
            }
 
            SearchTileNeighbors(tileToSearch, endPos);
        }
 
        Debug.Log("Aucun chemin trouvé.");
    }
 
    private void SearchTileNeighbors(Vector2 tilePos, Vector2 endPos)
    {
        for (float x = tilePos.x - 1; x <= tilePos.x + 1; x++)
        {
            for (float y = tilePos.y - 1; y <= tilePos.y + 1; y++)
            {
                Vector2 neighborPos = new Vector2(x, y);
 
                if (tiles.TryGetValue(neighborPos, out Tile neighbourTile) &&
                    !searchedTiles.Contains(neighborPos))
                {
                    float costToNeighbour = tiles[tilePos].distanceFromStart + Vector2.Distance(tilePos, neighborPos);
 
                    if (costToNeighbour < neighbourTile.distanceFromStart)
                    {
                        neighbourTile.connection = tilePos;
                        neighbourTile.distanceFromStart = costToNeighbour;
                        neighbourTile.estimatedDistanceToEnd = Vector2.Distance(neighborPos, endPos);
                        neighbourTile.totalEstimatedDistance = neighbourTile.distanceFromStart + neighbourTile.estimatedDistanceToEnd;

                        if (!tilesToSearch.Contains(neighborPos))
                        {
                            tilesToSearch.Add(neighborPos);
                            Debug.Log($"[Pathfinding] On ajoute {neighborPos} à la liste.");
                        }
                    }
                }
            }
        }
    }
    private void VisualiseText()
    {
        foreach (Transform child in textParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Vector2 pos in tiles.Keys)
        {
            Transform text = Instantiate(textPrefab, pos + (Vector2)transform.position, new Quaternion(), textParent);
            text.GetChild(0).GetComponent<Text>().text = tiles[pos].distanceFromStart.ToString();       
            text.GetChild(1).GetComponent<Text>().text = tiles[pos].estimatedDistanceToEnd.ToString();   
            text.GetChild(2).GetComponent<Text>().text = tiles[pos].totalEstimatedDistance.ToString();   
        }
    }
    
     private void OnDrawGizmos()
    {
        if (!visualiseGrid || tiles == null)
        {
            return;
        }

        foreach (KeyValuePair<Vector2, Tile> kvp in tiles)
        {

            if (finalPath.Contains(kvp.Key))
            {
                Gizmos.color = Color.magenta;
            }

            float gizmoSize = showTexts ? 0.2f : 1;

            Gizmos.DrawCube(kvp.Key + (Vector2)transform.position, new Vector3(tileWidth, tileHeight) * gizmoSize);
        }
    }


}
