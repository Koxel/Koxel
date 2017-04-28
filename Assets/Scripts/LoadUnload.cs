using UnityEngine;
using NiceJson;
using System.IO;

public class LoadUnload : MonoBehaviour {

    World World;
    public GameObject loader;
    GameObject currentChunk;
    GameObject prevChunk;
    int radius = 3;
    GameObject[] loaded;
    string path;

    void Start()
    {
        World = transform.FindChild("World").GetComponent<World>();
    }

    void Update()
    {
        if (loader != null)
        {
            GetCurrentChunk();
            if (currentChunk != prevChunk)
            {
                //Chunk was changed
                //ChunkToJSON(currentChunk);
            }
        }
    }

    void GetCurrentChunk()
    {
        RaycastHit hit;
        if (Physics.Raycast(loader.transform.position + new Vector3(0, 50, 0), -Vector3.up, out hit, Mathf.Infinity, 1 << 8))
        {
            prevChunk = currentChunk;
            currentChunk = hit.collider.transform.parent.parent.gameObject;
        }
    }

    void LoadChunk()
    {

    }

    void UnloadChunk()
    {
        
    }
    //Distance between neighbours = ~27.71;

    public void ChunkToJSON(GameObject chunk, string path)
    {
        JsonObject jsonObject = new JsonObject();
        jsonObject.Add("coords", new JsonArray { chunk.GetComponent<Chunk>().coords.x, chunk.GetComponent<Chunk>().coords.y });
        JsonArray tiles = new JsonArray();
        for(int child = 0; child < chunk.transform.childCount; child++)
        {
            Tile tile = chunk.transform.GetChild(child).GetComponent<Tile>();

            JsonArray tileObj = new JsonArray();
            tileObj.Add(tile.biome.Hname);
            tileObj.Add(tile.tileType.Hname);
            tiles.Add(tileObj);
        }
        jsonObject.Add("tiles", tiles);
        string json = jsonObject.ToJsonPrettyPrintString();

        string filePath = path + "/" + chunk.name + ".json";
        if (File.Exists(filePath))
            File.Delete(filePath);

        File.WriteAllText(filePath, json);
    }

    public void JSONToChunk(FileInfo jsonFile)
    {
        JsonObject json = (JsonObject)JsonNode.ParseJsonString(File.ReadAllText(jsonFile.FullName));
        Chunk chunk = World.CreateChunk(json["coords"][0], json["coords"][1]);
        int tileCount = 0;
        for (int r = -World.chunkRadius / 2; r < World.chunkRadius / 2; r++)
        {
            int r_offset = (int)Mathf.Floor(r / 2);
            for (int q = -World.chunkRadius / 2 - r_offset; q < World.chunkRadius / 2 - r_offset; q++)
            {
                //x=r, y=q
                Biome biome = World.FindBiome(json["tiles"][tileCount][0]);
                TileType tileType = biome.tileTypes[json["tiles"][tileCount][1]];
                World.CreateTile(r, q, chunk, biome, tileType);
                tileCount += 1;
            }
        }
    }
}
