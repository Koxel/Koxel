using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class World : MonoBehaviour {
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
    Map MAP;
    Dictionary<Vector2, Tile> map;
    Dictionary<string, Biome> biomes;
    
    public void SetupWorld(WorldData worldData, Dictionary<string, Biome> biomes)
    {
        MAP = GetComponent<Map>();
        hexWidth = Mathf.Sqrt(3) / 2 * hexHeight;

        WorldData = worldData;
        seed = (int)WorldData.seed;
        this.biomes = biomes;
    }

    public Biome FindBiome(string biomeName)
    {
        Biome biome;
        if (biomes.TryGetValue(biomeName, out biome))
            return biome;
        else
        {
            Debug.LogError("Biome '" + biomeName + "' cannot be found.");
            return null;
        }
    }
    public TileType FindTileType(Biome biome, string tileTypeName) 
    {
        TileType tileType;
        if (biome.tileTypes.TryGetValue(tileTypeName, out tileType))
            return tileType;
        else
        {
            Debug.LogError("TileType '" + tileTypeName + "' cannot be found in biome '" + biome.name + "'.");
            return null;
        }
    }

    public void Generate()
    {
        Debug.Log("Generating with seed: " + seed);
        map = new Dictionary<Vector2, Tile>();

        for (int r = -2; r < 5; r++)
        {
            for (int q = -5; q < 2; q++)
            {
                //FillChunkRandom(CreateChunk(r, q));
                PooledChunk(r,q, ObjectPooler.SharedInstance.GetPooledObject("Chunk"));
            }
        }
        MAP.currentTile = MAP.tileMap[new Vector2(0, 0)];
    }

    public Chunk PooledChunk(int x, int y, GameObject chunkObj)
    {
        // Position the chunk to the correct realPos
        Vector3 realPos = new Vector3();
        realPos.x = x * chunkRadius * hexWidth + y * (chunkRadius / 2 * hexWidth);
        realPos.z = y * chunkRadius * (0.75f * hexHeight);

        chunkObj.transform.parent = transform;
        chunkObj.name = "Chunk (" + x + ", " + y + ")";
        chunkObj.transform.position = realPos;
        chunkObj.SetActive(true);
        Chunk chunk = chunkObj.GetComponent<Chunk>();
        chunk.coords = new Vector2(x, y);
        MAP.chunkMap.Add(chunk.coords, chunk);
        chunk.neighbours = new List<Chunk>();
        MAP.UpdateChunkNeighbours(chunk);
        return chunk;
    }

    public Tile PooledTile(Tile tile, int x, int y, Chunk chunk, Biome biome, TileType tileType)
    {
        Vector3 realPos = new Vector3();
        realPos.x = x * hexWidth + 0.866026f * y;
        realPos.z = y * (hexHeight - (.25f * hexHeight));
        realPos = realPos + chunk.transform.position;
        // Turn relative chunkCoords into worldCoords
        int wX = (int)(chunk.GetComponent<Chunk>().coords.x * (chunkRadius) + x);
        int wY = (int)(chunk.GetComponent<Chunk>().coords.y * (chunkRadius) + y);
        int wZ = -wX - wY;
        // Do the height
        realPos.y = GetTileHeight(wX, wY);
        tile.transform.position = realPos;
        tile.name = "Tile (" + wX + ", " + wY + ")";
        tile.transform.parent = chunk.transform;

        // Save coords
        tile.chunkCoords = new Vector3(x, y, -x - y);
        tile.worldCoords = new Vector3(wX, wY, wZ);

        // Tile config
        if (biome == null)
            biome = GetBiome(realPos.y);
        tile.biome = biome;

        if (tileType == null)
            tileType = biome.tileTypes.ElementAt(0).Value;
        tile.tileType = tileType;

        tile.moveCost = tileType.moveCost;
        tile.SetColor(tile.tileType.defaultColor);

        tile.chunk = chunk;
        chunk.tiles.Add(new Vector2(x, y), tile);
        MAP.tileMap.Add(new Vector2(wX, wY), tile);
        //tile.neighbours = new List<Tile>();
        //MAP.GetNeighbours(tile);
        if (tile == null)
            Debug.LogError(tile + " is null!");
        if (textCoords) TextCoord(tile);
        return tile;
    }

    /*public Chunk CreateChunk(int x, int y)
    {
        // Position the chunk to the correct realPos
        Vector3 realPos = new Vector3();
        realPos.x = x * chunkRadius * hexWidth + y * (chunkRadius / 2 * hexWidth);
        realPos.z = y * chunkRadius * (0.75f * hexHeight);

        // Create chunk object
        var chunkObj = new GameObject();
        chunkObj.transform.parent = transform;
        chunkObj.name = "Chunk (" + x + ", " + y + ")";
        chunkObj.transform.position = realPos;
        Chunk chunk = chunkObj.AddComponent(typeof(Chunk)) as Chunk;
        chunk.coords = new Vector2(x, y);
        //Debug.Log(x + " " + y);
        MAP.chunkMap.Add(chunk.coords, chunk);
        chunk.neighbours = new List<Chunk>();
        MAP.UpdateChunkNeighbours(chunk);
        return chunk;
    }

    public Chunk FillChunkRandom(Chunk chunk)
    {
        for (int r = -chunkRadius / 2; r < chunkRadius / 2; r++)
        {
            int r_offset = (int)Mathf.Floor(r / 2);
            for (int q = -chunkRadius / 2 - r_offset; q < chunkRadius / 2 - r_offset; q++)
            {
                Tile tile = CreateTile(q, r, chunk, null, null);
            }
        }
        return chunk;
    }

    public Tile CreateTile(int x, int y, Chunk chunk, Biome biome, TileType tileType)
    {
        Vector3 realPos = new Vector3();
        realPos.x = x * hexWidth + 0.866026f * y;
        realPos.z = y * (hexHeight - (.25f * hexHeight));
        realPos = realPos + chunk.transform.position;
        // Turn relative chunkCoords into worldCoords
        int wX = (int)(chunk.GetComponent<Chunk>().coords.x * (chunkRadius) + x);
        int wY = (int)(chunk.GetComponent<Chunk>().coords.y * (chunkRadius) + y);
        int wZ = -wX - wY;
        // Do the height
        realPos.y = GetTileHeight(wX, wY);

        // Setup a new Tile
        //GameObject newTile = ObjectPooler.SharedInstance.GetPooledObject("Tile");
        //Debug.Log("Requested tile: " + newTile);
        GameObject newTile = Instantiate(TilePrefab, realPos, Quaternion.identity, chunk.transform);
        newTile.name = "Tile (" + wX + ", " + wY + ")";
        newTile.transform.parent = chunk.transform;

        // Save coords
        Tile tile = newTile.GetComponent<Tile>();
        tile.chunkCoords = new Vector3(x, y, -x -y);
        tile.worldCoords = new Vector3(wX, wY, wZ);

        // Tile config
        if (biome == null)
            biome = GetBiome(realPos.y);
        tile.biome = biome;

        if (tileType == null)
            tileType = biome.tileTypes.ElementAt(0).Value;
        tile.tileType = tileType;

        tile.moveCost = tileType.moveCost;
        tile.SetColor(tile.tileType.defaultColor);

        tile.chunk = chunk;
        chunk.tiles.Add(new Vector2(x, y), tile);
        MAP.tileMap.Add(new Vector2(tile.worldCoords.x, tile.worldCoords.y), tile);

        if (textCoords) TextCoord(tile);
        return tile;
    }*/
    
    Biome GetBiome(float height)
    {
        if (height < 20f)
            return biomes["ocean"];
        else if (height >= 20f && height < 35f)
            return biomes["forest"];
        else
            return biomes["mountain"];
    }

    float GetTileHeight(int x, int y)
    {
        // O.o #SuperNoise by @JohnyCilohokla.. lol
        x += seed;
        y += seed;

        float noise100 = Mathf.PerlinNoise(x / 100.0F, y / 100.0F);
        float noise1000 = Mathf.PerlinNoise(x / 1000.0F, y / 1000.0F);
        float noise10000 = Mathf.PerlinNoise(x / 10000.0F, y / 10000.0F);

        float ground = noise100 * 0.5F + noise1000 * 0.3F + noise10000 * 0.2F;


        float noiseM1 = Mathf.PerlinNoise(0.25F + x / 40.0F, 0.25F + y / 40.0F);
        float noiseM2 = Mathf.PerlinNoise(0.25F + x / 40.0F, 0.25F + y / 40.0F);
        float noiseM = Mathf.Sqrt(noiseM1) * Mathf.Sqrt(noiseM2);
        float mountain = 0;
        if (noiseM > 0.4)
        {
            mountain = (noiseM - 0.4f) * 0.4f;
        }



        float noiseN10 = Mathf.PerlinNoise(x / 5.0F, y / 5.0F);
        float noiseN = Mathf.PerlinNoise(0.25F + x / 25.0F, 0.25F + y / 25.0F);
        float noise = 0;
        if (noiseN > 0.55)
        {
            noise = Mathf.Max(0, noiseN10 - 0.5f) * 0.1f;
        }


        float noiseF = Mathf.PerlinNoise(0.85F + x / 40.0F, 0.85F + y / 40.0F);
        float ratioNoise = 1;
        if (noiseF > 0.5)
        {
            ratioNoise = 1 - Mathf.Max(0, (noiseF - 0.5f) * 0.5f);
        }

        float height = ratioNoise * (ground + mountain) + noise;


        if (flat) height = 0;
        height *= 50;
        return height;
    }

    public GameObject TextObj;
    public bool textCoords;
    void TextCoord(Tile tile)
    {
        string coordtext = tile.worldCoords.x + "," + tile.worldCoords.y;
        GameObject obj = Instantiate(TextObj, tile.transform.position, Quaternion.identity, tile.transform);
        obj.transform.GetChild(0).GetComponent<TextMesh>().text = coordtext;
    }

    public GameObject playerPrefab;
    public GameObject CreatePlayer()
    {
        GameObject player = Instantiate(playerPrefab, MAP.tileMap[new Vector2(0, 0)].transform.position, Quaternion.identity, transform.parent);
        player.name = "Player";
        MAP.currentTile = MAP.tileMap[new Vector2(0, 0)];
        return player;
    }


    public WorldData WorldData;
}

public class WorldData
{
    public string name;
    public float seed;
}