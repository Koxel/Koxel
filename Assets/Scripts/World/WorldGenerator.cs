using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public int width = 64;
    public int depth = 64;
    public int heightScale = 20;
    public float detailScale = 25.0f;

    public float hexOffsetX = 1.8f;
    public float hexOffsetY = 1.524116f;
    public float hexOddRowOffsetX = 0.9f;

    public GameObject Tile;
    public TileBehaviour[,] tileMap;

    void Start()
    {
        
    }

    public void Generate(List<Tile> tilesList)
    {
        int seed = (int)Random.Range(10000000, 30000000);
        Debug.Log("Seed: " + seed);

        tileMap = new TileBehaviour[width, depth];

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int y = (int)(Mathf.PerlinNoise((x + seed) / detailScale, (z + seed) / detailScale) * heightScale);

                Vector3 tilePos = new Vector3();
                if (z % 2 == 0 || z == 0) //Even
                    tilePos = new Vector3(x * hexOffsetX, y, z * hexOffsetY);
                else if (z % 2 == 1) //Odd
                    tilePos = new Vector3(x * hexOffsetX + hexOddRowOffsetX, y, z * hexOffsetY);

                // Setup a new Tile
                GameObject newTile = Instantiate(Tile, tilePos, Quaternion.identity);
                newTile.transform.parent = transform;
                newTile.name = "Tile (" + x + ", " + z + ")";

                // Save the coords
                TileBehaviour tb = newTile.GetComponent<TileBehaviour>();
                tb.coordinates = new Vector3(x, y, z);
                tileMap[x, z] = tb;

                // Colours!
                if (y > 8)
                    newTile.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(49f/255f, 159f/255f, 99f/255f, 255f/255f);
            }
        }

        // Setup the map stuff
        Map map = GetComponent<Map>();
        GameObject player = Instantiate(map.playerPrefab, tileMap[width/2, depth/2].transform.position, Quaternion.identity);
        player.name = "Player";
        map.player = player;
        map.tileMap = tileMap;
        map.currentTile = tileMap[width / 2, depth / 2];
        Debug.Log("Finished Generation");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}