using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    // Generation settings
    public int chunkRadius = 7;
    public bool flat = false;
    public bool customSeed;
    public int seed;
    public int heightScale = 20;
    public float detailScale = 25.0f;

    // Prefabs
    public GameObject TilePrefab;

    // Calculated distances
    public float hexHeight = 2.00f;
    private float hexWidth;
    private float chunkWidth;
    private float chunkHeight;
    
    // Maps
    private Dictionary<Vector2, TileBehaviour> map;

    // Will be called after JSON gets parsed
    public void Generate(List<Tile> tilesList)
    {
        // Calculate the distances
        hexWidth = Mathf.Sqrt(3) / 2 * hexHeight;
        chunkWidth = (chunkRadius * 2 + 1) * (1.25f * hexWidth);
        chunkHeight = ((chunkRadius * 2 + 1) * (0.75f * hexHeight));

        // No custom seed? Calculate one!
        if (!customSeed) seed = Random.Range(1, 99999999);
        map = new Dictionary<Vector2, TileBehaviour>();
        
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                NewChunk(i, j);
            }
        }

        // Setup the map stuff
        Map tilemap = GetComponent<Map>();
        GameObject player = Instantiate(tilemap.playerPrefab, map[new Vector2(0, 0)].transform.position, Quaternion.identity);
        player.name = "Player";
        tilemap.player = player;
        tilemap.tileMap = map;
        tilemap.currentTile = map[new Vector2(0, 0)];
        Debug.Log("Finished Initial Generation");
    }

    void NewChunk(int x, int y)
    {
        // Position the chunk to the correct realPos
        Vector3 realPos = new Vector3();
        
        if (x % 2 == -1 || x % 2 == 1)
        {
            realPos.x = x * (0.5939f * chunkWidth) + y * (0.5f * hexWidth);
            realPos.z = y * (chunkHeight) + (0.495f * chunkHeight) + 0.5f * hexHeight;
        }
        else
        {
            realPos.x = x * (0.5939f * chunkWidth) + y * (0.5f * hexWidth);
            realPos.z = y * (chunkHeight);
        }

        // Create chunk object
        var chunkObj = new GameObject();
        chunkObj.name = "Chunk (" + x + ", " + y + ")";
        chunkObj.transform.position = realPos;
        Chunk chunk = chunkObj.AddComponent(typeof(Chunk)) as Chunk;
        chunk.coords = new Vector2(x, y);
        chunk = CreateChunk(chunk, chunkObj);
    }

    Chunk CreateChunk(Chunk chunk, GameObject chunkObj)
    {
        chunk.tiles = new List<TileBehaviour>();

        for (int q = -chunkRadius; q <= chunkRadius; q++)
        {
            int r1 = Mathf.Max(-chunkRadius, -q - chunkRadius);
            int r2 = Mathf.Min(chunkRadius, -q + chunkRadius);
            for (int r = r1; r <= r2; r++)
            {
                TileBehaviour tile = CreateTile(q, r, chunkObj);
                chunk.tiles.Add(tile);
            }
        }

        return chunk;
    }

    TileBehaviour CreateTile(int x, int y, GameObject chunk)
    {
        // Calculate the realPos
        Vector3 realPos = new Vector3();
        // Turn relative chunkCoords into worldCoords
        //int wX = (int)(chunk.GetComponent<Chunk>().coords.x * (chunkRadius * 2 + 1)) + x;
        //int wY = (int)(chunk.GetComponent<Chunk>().coords.y * (chunkRadius * 2 + 1)) + y;
        
        // 2D coords
        realPos.x = x * hexWidth + y - (y * 0.133974f);
        realPos.z = y * (hexHeight - (.25f * hexHeight));
        realPos = realPos + chunk.transform.position;
        int wX = (int) (realPos.x / hexWidth);
        int wY = (int) (realPos.z / hexHeight - (.25f * hexHeight));
        // FUCK
        /*int wX = (int)(chunk.GetComponent<Chunk>().coords.x + x);
        int wY = (int)(chunk.GetComponent<Chunk>().coords.y + y);*/
        int wZ = -wX - wY;

        // Do the height
        realPos.y = GetTileHeight(wX, wY);
        
        // Setup a new Tile
        GameObject newTile = Instantiate(TilePrefab, realPos, Quaternion.identity);
        newTile.transform.parent = chunk.transform;

        // Save to the map
        TileBehaviour tile = newTile.GetComponent<TileBehaviour>();
        tile.chunkCoords = new Vector3(x, y, -x -y);
        
        tile.worldCoords = new Vector3(wX, wY, wZ);
        tile.moveCost = 1;
        newTile.name = "Tile (" + tile.worldCoords.x + ", " + tile.worldCoords.y + ")";

        return tile;
    }

    void SetTileNeighbours(List<TileBehaviour> newTiles)
    {
        // Set neighbours
        foreach (TileBehaviour tile in newTiles)
        {   //  0, -1
            if (map.ContainsKey(new Vector2(tile.worldCoords.x, tile.worldCoords.y - 1)))
                tile.neighbours.Add(map[new Vector2(tile.worldCoords.x, tile.worldCoords.y - 1)]);
            // +1, -1
            if (map.ContainsKey(new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y - 1)))
                tile.neighbours.Add(map[new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y - 1)]);
            // -1,  0
            if (map.ContainsKey(new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y)))
                tile.neighbours.Add(map[new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y)]);
            // +1,  0
            if (map.ContainsKey(new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y)))
                tile.neighbours.Add(map[new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y)]);
            // -1, +1
            if (map.ContainsKey(new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y + 1)))
                tile.neighbours.Add(map[new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y + 1)]);
            //  0, +1
            if (map.ContainsKey(new Vector2(tile.worldCoords.x, tile.worldCoords.y + 1)))
                tile.neighbours.Add(map[new Vector2(tile.worldCoords.x, tile.worldCoords.y + 1)]);
        }
    }

    void SetTileColour(TileBehaviour tile)
    {
        // Colours!
        if (tile.chunkCoords.y > 8)
        {
            tile.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(49f / 255f, 159f / 255f, 99f / 255f, 255f / 255f);
            //tb.moveCost = 50;
        }
    }

    float GetTileHeight(int x, int y)
    {
        int height = (int)(Mathf.PerlinNoise((x + seed) / detailScale, (y + seed) / detailScale) * heightScale);
        if (flat) height = 0;
        return height;
    }
}