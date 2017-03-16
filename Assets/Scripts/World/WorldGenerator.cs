using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (!customSeed) seed = Random.Range(1, 3000000);
        map = new Dictionary<Vector2, TileBehaviour>();

        for (int r = 0; r < 4; r++)
        {
            for (int q = 0; q < 4; q++)
            {
                NewChunk(r, q);
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
        
        realPos.x = x * chunkRadius * hexWidth + y * (chunkRadius/2 * hexWidth);
        // Square chunk
        //realPos.x = x * chunkRadius * hexWidth;
        realPos.z = y * chunkRadius * (0.75f * hexHeight);

        // Hexagonal Chunk
        /*if (x % 2 == -1 || x % 2 == 1)
        {
            realPos.x = x * (0.5939f * chunkWidth) + y * (0.5f * hexWidth);
            realPos.z = y * (chunkHeight) + (0.495f * chunkHeight) + 0.5f * hexHeight;
        }
        else
        {
            realPos.x = x * (0.5939f * chunkWidth) + y * (0.5f * hexWidth);
            realPos.z = y * (chunkHeight);
        }*/

        // Create chunk object
        var chunkObj = new GameObject();
        chunkObj.transform.parent = transform;
        chunkObj.name = "Chunk (" + x + ", " + y + ")";
        chunkObj.transform.position = realPos;
        Chunk chunk = chunkObj.AddComponent(typeof(Chunk)) as Chunk;
        chunk.coords = new Vector2(x, y);
        chunk = CreateChunk(chunk, chunkObj);
    }

    Chunk CreateChunk(Chunk chunk, GameObject chunkObj)
    {
        chunk.tiles = new List<TileBehaviour>();
        
        // Square chunk
        for (int r = -chunkRadius / 2; r < chunkRadius / 2; r++)
        {
            int r_offset = (int)Mathf.Floor(r / 2);
            for (int q = -chunkRadius / 2 - r_offset; q < chunkRadius / 2 - r_offset; q++)
            {
                TileBehaviour tile = CreateTile(q, r, chunkObj, r_offset);
                chunk.tiles.Add(tile);
            }
        }

        // Hexagon Chunk
        /*
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
        */
        return chunk;
    }

    TileBehaviour CreateTile(int x, int y, GameObject chunk, int r_offset)
    {
        // Calculate the realPos
        Vector3 realPos = new Vector3();
        // 2D coords
        realPos.x = x * hexWidth + 0.866026f * y;
        realPos.z = y * (hexHeight - (.25f * hexHeight));
        realPos = realPos + chunk.transform.position;
        // Turn relative chunkCoords into worldCoords
        int wX = (int)(chunk.GetComponent<Chunk>().coords.x * (chunkRadius) + x);
        int wY = (int)(chunk.GetComponent<Chunk>().coords.y * (chunkRadius) + y);
        int wZ = -wX - wY;
        // Do the height
        realPos.y = GetTileHeight(wX, wY);
        TextCoord(realPos, wX, wY);
        // Setup a new Tile
        GameObject newTile = Instantiate(TilePrefab, realPos, Quaternion.identity);
        newTile.transform.parent = chunk.transform;

        // Save to the map
        TileBehaviour tile = newTile.GetComponent<TileBehaviour>();
        tile.chunkCoords = new Vector3(x, y, -x -y);
        
        tile.worldCoords = new Vector3(wX, wY, wZ);
        tile.moveCost = 1;
        newTile.name = "Tile (" + wX + ", " + wY + ")";
       
        map.Add(new Vector2(wX, wY), tile);
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

    int PixelRound(float nr)
    {
        bool negative = false;
        if(nr < 0)
        {
            negative = true;
            nr *= -1;
        }
        double decimals = nr - System.Math.Truncate(nr);
        double remainder = 1f - (float)decimals;
        int rounded;
        if (decimals >= 0.49)
            rounded = (int)(nr + remainder);
        else
            rounded = (int)(nr - decimals);
        if (negative)
            rounded *= -1;

        return rounded;
    }

    public GameObject TextObj;
    public bool textCoords;
    void TextCoord(Vector3 position, int x, int y)
    {
        if (textCoords)
        {
            string coordtext = x + "," + y;
            GameObject obj = Instantiate(TextObj, position, Quaternion.identity);
            obj.transform.GetChild(0).GetComponent<TextMesh>().text = coordtext;
        }
    }
}