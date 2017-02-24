using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    public bool flat = false;
    public int radius = 16;
    public int heightScale = 20;
    public float detailScale = 25.0f;
    
    public float hexHeight = 2.00f;
    float hexWidth;

    public GameObject Tile;
    public Dictionary<Vector2, TileBehaviour> mapDict;
    List<TileBehaviour> tileList;
    public bool customSeed;
    public int seed;

    public void Generate(List<Tile> tilesList)
    {
        // Calc hex
        hexWidth = Mathf.Sqrt(3) / 2 * hexHeight;

        // No custom seed? Calculate one!
        if(!customSeed) seed = Random.Range(1, 99999999);

        tileList = new List<TileBehaviour>();
        mapDict = new Dictionary<Vector2, TileBehaviour>();

        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                int height = (int)(Mathf.PerlinNoise((q + seed) / detailScale, (r + seed) / detailScale) * heightScale);
                if(flat) height = 0;

                Vector3 tilePos = new Vector3();        // v FIX THIS v
                tilePos = new Vector3(q * hexWidth + r - (r * 0.133974f), height, r * (hexHeight - (.25f * hexHeight)));

                // Setup a new Tile
                GameObject newTile = Instantiate(Tile, tilePos, Quaternion.identity);
                newTile.transform.parent = transform;
                newTile.name = "Tile (" + q + ", " + r + ")";

                // Save to the map
                TileBehaviour tb = newTile.GetComponent<TileBehaviour>();
                tb.coordinates = new Vector3(q, r, -q-r);
                mapDict.Add(new Vector2(q, r), tb);
                tileList.Add(tb);

                // Colours!
                if (height > 8)
                    newTile.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(49f / 255f, 159f / 255f, 99f / 255f, 255f / 255f);
            }
        }

        // Find neighbours
        /// Neighbours: 
        ///  0, -1 | +1, -1
        /// -1,  0 | +1,  0
        /// -1, +1 |  0, +1
        foreach (TileBehaviour tile in tileList)
        {   //  0, -1
            if(mapDict.ContainsKey(new Vector2(tile.coordinates.x, tile.coordinates.y - 1)))
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

        // Setup the map stuff
        Map map = GetComponent<Map>();
        GameObject player = Instantiate(map.playerPrefab, mapDict[new Vector2(0, 0)].transform.position, Quaternion.identity);
        player.name = "Player";
        map.player = player;
        map.tileMap = mapDict;
        map.radius = radius;
        map.currentTile = mapDict[new Vector2(0, 0)];
        Debug.Log("Finished Generation");
        //map.BreadthFirstSearch();
    }
}