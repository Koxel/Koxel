using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private Dictionary<Vector2, Tile> map;

    // Will be called after JSON gets parsed
    public void Generate(Dictionary<string, Biome> biomes)
    {
        hexWidth = Mathf.Sqrt(3) / 2 * hexHeight;

        // No custom seed? Calculate one!
        if (!customSeed) seed = Random.Range(1000000, 3000000);
        map = new Dictionary<Vector2, Tile>();

        for (int r = 0; r < 4; r++)
        {
            for (int q = 0; q < 4; q++)
            {
                NewChunk(r, q, biomes.ElementAt(Random.Range(0, biomes.Count)).Value);
            }
        }

        // Setup the map stuff
        Map tilemap = GetComponent<Map>();
        GameObject player = Instantiate(tilemap.playerPrefab, map[new Vector2(0, 0)].transform.position, Quaternion.identity);
        player.name = "Player";
        player.GetComponent<Controls>().map = tilemap;
        tilemap.player = player;
        tilemap.tileMap = map;
        tilemap.currentTile = map[new Vector2(0, 0)];
        tilemap.playerCam.SetPlayer(player);
        tilemap.playerCam.transform.position = player.transform.position + tilemap.playerCam.offset;
        Debug.Log("Finished Initial Generation");
    }

    void NewChunk(int x, int y, Biome biome)
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
        chunk.biome = biome;
        chunk.BiomeName = chunk.biome.name;
        Debug.Log("Biome: " + chunk.biome.name);
        chunk = CreateChunk(chunkObj);
    }

    Chunk CreateChunk(GameObject chunkObj)
    {
        Chunk chunk = chunkObj.GetComponent<Chunk>();
        chunk.tiles = new List<Tile>();
        
        for (int r = -chunkRadius / 2; r < chunkRadius / 2; r++)
        {
            int r_offset = (int)Mathf.Floor(r / 2);
            for (int q = -chunkRadius / 2 - r_offset; q < chunkRadius / 2 - r_offset; q++)
            {
                Tile tile = CreateTile(q, r, chunkObj);
                chunk.tiles.Add(tile);
            }
        }
        return chunk;
    }

    Tile CreateTile(int x, int y, GameObject chunk)
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

        // Setup a new Tile
        GameObject newTile = Instantiate(TilePrefab, realPos, Quaternion.identity);
        newTile.transform.parent = chunk.transform;
        newTile.name = "Tile (" + wX + ", " + wY + ")";

        // Save to the map
        Tile tile = newTile.GetComponent<Tile>();
        tile.chunkCoords = new Vector3(x, y, -x -y);
        tile.worldCoords = new Vector3(wX, wY, wZ);
        // Tile config
        Biome biome = chunk.GetComponent<Chunk>().biome;
        
        tile.tileType = biome.tiles[0];
        tile.moveCost = 1;
        tile.SetColor(tile.tileType.defaultColor);

        if (textCoords) TextCoord(tile);
        map.Add(new Vector2(wX, wY), tile);
        return tile;
    }

    float GetTileHeight(int x, int y)
    {
        int height = (int)(Mathf.PerlinNoise((x + seed) / detailScale, (y + seed) / detailScale) * heightScale);
        if (flat) height = 0;
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
}