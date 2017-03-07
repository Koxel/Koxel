using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    public bool flat = false;
    public int radius = 16;
    public int chunkRadius = 7;
    public int heightScale = 20;
    public float detailScale = 25.0f;
    public float width = 3f;
    public float depth = 3f;
    
    public float hexHeight = 2.00f;
    float hexWidth;
    
    public GameObject Tile;
    public Dictionary<Vector2, TileBehaviour> mapDict;
    public bool customSeed;
    public int seed;

    public void Generate(List<Tile> tilesList)
    {
        // Calc hex
        hexWidth = Mathf.Sqrt(3) / 2 * hexHeight;

        // Calc chunk
        float chunkWidth = (chunkRadius * 2) + 1;
        float chunkHeight = Mathf.Sqrt(3) / 2 * chunkWidth;
        float act_chunkWidth = chunkWidth * hexHeight;
        float act_chunkHeight = chunkHeight * hexWidth;

        float Xoffset = hexHeight;
        float Yoffset = hexWidth;
        float XoddOffset = .5f * hexWidth;

        // No custom seed? Calculate one!
        if (!customSeed) seed = Random.Range(1, 99999999);
        mapDict = new Dictionary<Vector2, TileBehaviour>();

        /*NewChunk(0, 0);
        NewChunk(0, -1);
        NewChunk(1, -1);
        NewChunk(1, 0);
        NewChunk(-1, 0);
        NewChunk(-1, 1);
        NewChunk(0, 1);*/
        for (int i = -2; i < 2; i++)
        {
            for (int j = -3; j < 3; j++)
            {
                NewChunk(i, j);
            }
        }

        // Setup the map stuff
        Map map = GetComponent<Map>();
        GameObject player = Instantiate(map.playerPrefab, mapDict[new Vector2(0, 0)].transform.position, Quaternion.identity);
        player.name = "Player";
        map.player = player;
        map.tileMap = mapDict;
        map.radius = radius;
        map.currentTile = mapDict[new Vector2(0, 0)];
        //Debug.Log("Finished Initial Generation");
    }

    void NewChunk(int x, int y)
    {
        // Calc chunk
        float chunkWidth = (chunkRadius * 2 + 1) * (1.25f * hexWidth);
        float chunkHeight = ((chunkRadius * 2 + 1) * (0.75f * hexHeight));

        Vector3 pos = new Vector3();
        pos.y = 0f;
        
        // Change this part for the chunk positioning!!!
        if (x % 2 == -1)
        {
            pos.x = x * (0.75f * chunkWidth) + y * (0.5f * hexWidth);
            pos.z = y * (chunkHeight) + (0.5f * chunkHeight) + 0.5f * hexHeight;
        }
        else if (x % 2 == 1)
        {
            pos.x = x * (0.75f * chunkWidth) + y * (0.5f * hexWidth);
            pos.z = y * (chunkHeight) - (0.5f * chunkHeight) - 0.5f * hexHeight;
        }
        else
        {
            pos.x = x * (0.75f * chunkWidth) + y * (0.5f * hexWidth);
            pos.z = y * (chunkHeight);
        }

        var chunkObj = new GameObject();
        chunkObj.name = "Chunk (" + x + ", " + y + ")";
        chunkObj.transform.position = pos;
        Chunk chunk = new Chunk();
        chunk = CreateChunk(chunk, chunkObj, new Vector2(x, y));
    }

    public Chunk CreateChunk(Chunk chunk, GameObject chunkObj, Vector2 mapPos)
    {
        float chunkWidth = (chunkRadius * 2) + 1;
        float chunkHeight = Mathf.Sqrt(3) / 2 * chunkWidth;
        float act_chunkWidth = chunkWidth * hexHeight;
        float act_chunkHeight = chunkHeight * hexWidth;

        chunk.tiles = new List<TileBehaviour>();

        Vector2 worldCoords = new Vector2(chunk.coords.x * act_chunkWidth, chunk.coords.y * act_chunkHeight);

        for (int q = -chunkRadius; q <= chunkRadius; q++)
        {
            int r1 = Mathf.Max(-chunkRadius, -q - chunkRadius);
            int r2 = Mathf.Min(chunkRadius, -q + chunkRadius);
            for (int r = r1; r <= r2; r++)
            {
                int height = (int)(Mathf.PerlinNoise((q + seed) / detailScale, (r + seed) / detailScale) * heightScale);
                if (flat) height = 0;

                Vector3 tilePos = new Vector3();
                tilePos.y = height;

                tilePos.x = q * hexWidth + r - (r * 0.133974f);
                tilePos.z = r * (hexHeight - (.25f * hexHeight));

                //tilePos = new Vector3(q * hexWidth + r - (r * 0.133974f), height, r * (hexHeight - (.25f * hexHeight)));

                // Setup a new Tile
                GameObject newTile = Instantiate(Tile, tilePos + chunkObj.transform.position, Quaternion.identity);
                newTile.transform.parent = chunkObj.transform;
                newTile.name = "Tile (" + q + ", " + r + ")";

                // Save to the map
                TileBehaviour tb = newTile.GetComponent<TileBehaviour>();
                tb.coordinates = new Vector3(q, r, -q - r);
                tb.moveCost = 1;
                //mapDict.Add(new Vector2(q, r), tb);
                chunk.tiles.Add(tb);

                // Colours!
                if (height > 8)
                {
                    newTile.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(49f / 255f, 159f / 255f, 99f / 255f, 255f / 255f);
                    //tb.moveCost = 50;
                }
            }
        }

        // Set neighbours
        foreach (TileBehaviour tile in chunk.tiles)
        {   //  0, -1
            if (mapDict.ContainsKey(new Vector2(tile.coordinates.x, tile.coordinates.y - 1)))
                tile.neighbours.Add(mapDict[new Vector2(tile.coordinates.x, tile.coordinates.y - 1)]);
            // +1, -1
            if (mapDict.ContainsKey(new Vector2(tile.coordinates.x + 1, tile.coordinates.y - 1)))
                tile.neighbours.Add(mapDict[new Vector2(tile.coordinates.x + 1, tile.coordinates.y - 1)]);
            // -1,  0
            if (mapDict.ContainsKey(new Vector2(tile.coordinates.x - 1, tile.coordinates.y)))
                tile.neighbours.Add(mapDict[new Vector2(tile.coordinates.x - 1, tile.coordinates.y)]);
            // +1,  0
            if (mapDict.ContainsKey(new Vector2(tile.coordinates.x + 1, tile.coordinates.y)))
                tile.neighbours.Add(mapDict[new Vector2(tile.coordinates.x + 1, tile.coordinates.y)]);
            // -1, +1
            if (mapDict.ContainsKey(new Vector2(tile.coordinates.x - 1, tile.coordinates.y + 1)))
                tile.neighbours.Add(mapDict[new Vector2(tile.coordinates.x - 1, tile.coordinates.y + 1)]);
            //  0, +1
            if (mapDict.ContainsKey(new Vector2(tile.coordinates.x, tile.coordinates.y + 1)))
                tile.neighbours.Add(mapDict[new Vector2(tile.coordinates.x, tile.coordinates.y + 1)]);
        }
        return chunk;
    }
}