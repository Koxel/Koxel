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
        player.GetComponent<Controls>().map = tilemap;
        tilemap.player = player;
        tilemap.tileMap = map;
        tilemap.currentTile = map[new Vector2(0, 0)];
        tilemap.playerCam.SetPlayer(player);
        tilemap.playerCam.transform.position = player.transform.position + tilemap.playerCam.offset;
        Debug.Log("Finished Initial Generation");
    }

    void NewChunk(int x, int y)
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
        chunk = CreateChunk(chunk, chunkObj);
    }

    Chunk CreateChunk(Chunk chunk, GameObject chunkObj)
    {
        chunk.tiles = new List<TileBehaviour>();
        
        for (int r = -chunkRadius / 2; r < chunkRadius / 2; r++)
        {
            int r_offset = (int)Mathf.Floor(r / 2);
            for (int q = -chunkRadius / 2 - r_offset; q < chunkRadius / 2 - r_offset; q++)
            {
                TileBehaviour tile = CreateTile(q, r, chunkObj, r_offset);
                chunk.tiles.Add(tile);
            }
        }
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
        
        // Setup a new Tile
        GameObject newTile = Instantiate(TilePrefab, realPos, Quaternion.identity);
        newTile.transform.parent = chunk.transform;
        newTile.name = "Tile (" + wX + ", " + wY + ")";

        // Save to the map
        TileBehaviour tile = newTile.GetComponent<TileBehaviour>();
        tile.chunkCoords = new Vector3(x, y, -x -y);
        tile.worldCoords = new Vector3(wX, wY, wZ);
        tile.moveCost = 1;
        if(textCoords) TextCoord(tile);
        map.Add(new Vector2(wX, wY), tile);
        return tile;
    }

    void SetTileColour(TileBehaviour tile)
    {
        if (tile.chunkCoords.y > 8)
        {
            tile.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(49f / 255f, 159f / 255f, 99f / 255f, 255f / 255f);
        }
    }

    float GetTileHeight(int x, int y)
    {
        int height = (int)(Mathf.PerlinNoise((x + seed) / detailScale, (y + seed) / detailScale) * heightScale);
        if (flat) height = 0;
        return height;
    }

    public GameObject TextObj;
    public bool textCoords;
    void TextCoord(TileBehaviour tile)
    {
        string coordtext = tile.worldCoords.x + "," + tile.worldCoords.y;
        GameObject obj = Instantiate(TextObj, tile.transform.position, Quaternion.identity, tile.transform);
        obj.transform.GetChild(0).GetComponent<TextMesh>().text = coordtext;
    }
}