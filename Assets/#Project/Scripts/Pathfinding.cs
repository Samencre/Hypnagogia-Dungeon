using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Pathfinding : MonoBehaviour {
    [SerializeField] private bool newCaculPath;
    [SerializeField] private bool pathGenerated;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private int tileWidth = 1;
    [SerializeField] private int tileHeight = 1;
    [SerializeField] private Tilemap groundTilemap;
    private Dictionary<Vector2, Tile> tiles;
    // Tuiles praticable
    private List<Vector2> tilesToSearch;
    // Liste ouverte (a explorer)
    private List<Vector2> searchedTiles;
    // Liste fermer (deja explorées)
    private List<Vector2> finalPathFind;

    public Transform nightmare;
    public Transform player;
    public float distanceStartChase = 4f;
    public float distanceStopChase = 5f;
    public float speedMovement = 2f;
    public Rigidbody2D rbNightmare;

    private bool isChasing = false;
    private Coroutine followCoroutine; 
    // Coroutine qui active le deplacement
    
    [SerializeField] private bool visualiseGrid;
    [SerializeField] private bool showTexts;

    
    void Awake()
    {
        gridWidth = groundTilemap.size.x;
        gridHeight = groundTilemap.size.y;
        // Recupere la taille de la tilemap
    }
    
    private void Update()
    {
        tiles = new Dictionary<Vector2, Tile>();
        Vector3Int playerGridPos = groundTilemap.layoutGrid.WorldToCell(player.position) - groundTilemap.origin;
        Vector3Int nightmareGridPos = groundTilemap.layoutGrid.WorldToCell(nightmare.position) - groundTilemap.origin;
        // Conversion des positions monde en positions cellules (pas encore tres claire...)

        CaculPath(new Vector2(nightmareGridPos.x, nightmareGridPos.y), new Vector2(playerGridPos.x, playerGridPos.y));
                
        if (player == null || nightmare == null) return;
        // Verification d'assignation

        float distanceBetweenPlayerNightmare = Vector2.Distance(nightmare.position, player.position);

        // Player entre dans la zone de poursuite
        if (!isChasing && distanceBetweenPlayerNightmare < distanceStartChase)
        {
            isChasing = true;
            newCaculPath = true;
        }

        // Playeur sort de la zone de poursuite
        else if (isChasing && distanceBetweenPlayerNightmare > distanceStopChase)
        {
            isChasing = false;
            newCaculPath = false;

            // Arrete la coroutine de suivi si elle est active
            if (followCoroutine != null)
            {
                StopCoroutine(followCoroutine);
                followCoroutine = null;
            }
        }
    
        if (isChasing)
        {
            
            if (newCaculPath && !pathGenerated)
            {
                GenerateGrid(); 
                
                if (finalPathFind != null && finalPathFind.Count > 0)
                {
                    // Stoppe l'ancienne coroutine avant d’en lancer une nouvelle
                    if (followCoroutine != null) StopCoroutine(followCoroutine);
                    followCoroutine = StartCoroutine(FollowPath());
                }
    
                newCaculPath = false;
                pathGenerated = true;
            }
            
            else if (!newCaculPath)
            {
                pathGenerated = false;
            }
        }
    }
    
    // Suis pas en paix avec cette fonction la...
    private void GenerateGrid()
    {
        tiles.Clear();
        // Parcourt toutes les cellules de la tilemap pour reperer les tuiles praticable

        Vector3Int origin = groundTilemap.origin;
        Vector3Int size = groundTilemap.size;
        //Origine de la tilmap
        
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(origin.x + x, origin.y + y, 0);
                //calcul la position reel de la tuile dans la grille 

                if (groundTilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    Vector2 gridPos = new Vector2(cellPos.x, cellPos.y);
                    // Vector2 gridPos = new Vector2(x, y);
                    Tile tile = new Tile();
                    tile.position = gridPos;
                    tile.distanceFromStart = Mathf.Infinity;
                    tile.estimatedDistanceToEnd = Mathf.Infinity;
                    tile.totalEstimatedDistance = Mathf.Infinity;
                    // Mathf.Infinity (ramplace float.MaxValue et "null" qu'on ne peut pas mettre a un float (si j'ai bien compris))
                    tiles.Add(gridPos, tile);
                }
            }
        }
    }
    
    private void CaculPath(Vector2 startPos, Vector2 endPos)
    {
        tilesToSearch = new List<Vector2> { startPos };
        // A explorer
        searchedTiles = new List<Vector2>();
        // Deja explorées
        finalPathFind = new List<Vector2>();

        Debug.Log($"tiles: {tiles}");
        tiles[startPos].distanceFromStart = 0;
        tiles[startPos].estimatedDistanceToEnd = Vector2.Distance(startPos, endPos);
        tiles[startPos].totalEstimatedDistance = tiles[startPos].estimatedDistanceToEnd;
        // Pas claire ....

        // A explorer
        while (tilesToSearch.Count > 0)
        {
            Vector2 tileToSearch = tilesToSearch[0];
            // Choisi la tuile avec la plus petit valeur

            // Toujour pas clair...
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

            // La tuile passe de "A explorer" a " Deja explorer"
            tilesToSearch.Remove(tileToSearch);
            searchedTiles.Add(tileToSearch);

            // Si la destination est atteind, on cree un nouveau chemin
            if (tileToSearch == endPos)
            {
                Tile pathTile = tiles[endPos];

                while (pathTile.position != startPos)
                {
                    finalPathFind.Add(pathTile.position);
                    pathTile = tiles[pathTile.connection];
                }
                finalPathFind.Add(startPos);
                // finalPathFind.Reverse();
                // Met le chemin dans le bon sens et non c'est pas claire.....
                Debug.Log($"Chemin trouvé ! Longueur : {finalPathFind.Count}");
                return;
            }

            // exploration des tuiles voisines
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
    
                // Ignore la tuile centrale
                if (neighborPos == tilePos) continue;
    
                // Si la tuile existe et n’est pas "Deja explorer"
                if (tiles.TryGetValue(neighborPos, out Tile neighbourTile) && !searchedTiles.Contains(neighborPos))
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
    
    
    private IEnumerator FollowPath()
    {
        // Tant qu’il y a un chemin et que le player est poursuivi
        foreach (Vector2 step in finalPathFind)
        {
            // Conversion en position monde (toujour pas clair...)
            Vector3 targetWorldPos = groundTilemap.CellToWorld(new Vector3Int((int)step.x + groundTilemap.origin.x, (int)step.y + groundTilemap.origin.y, 0)) + (Vector3)groundTilemap.cellSize / 2f;
    
            // Avance vers le point jusqua O.O5f
            while (Vector2.Distance(nightmare.position, targetWorldPos) > 0.05f)
            {
                // Si le player sort de la zone de poursuite, on arrete la coroutine
                if (!isChasing) yield break;
    
                nightmare.position = Vector2.MoveTowards(nightmare.position, targetWorldPos, speedMovement * Time.deltaTime);
                yield return null; 
                // Ok je comprend pas grand chose...
            }
        }
    }
    
    
    private void OnDrawGizmos()
    {
        if (!visualiseGrid || tiles == null) return;
    
        foreach (KeyValuePair<Vector2, Tile> kvp in tiles)
        {
            if (finalPathFind != null && finalPathFind.Contains(kvp.Key)) Gizmos.color = Color.magenta;
            else Gizmos.color = Color.gray;
    
            float gizmoSize = showTexts ? 0.2f : 1f;
            Gizmos.DrawCube(kvp.Key + (Vector2)transform.position, new Vector3(tileWidth, tileHeight) * gizmoSize);
        }
    }
}