using UnityEngine;
using NiceJson;
using System.IO;
using System.Collections.Generic;

public class LoadUnload : MonoBehaviour {

    Game Game;
    World World;
    Map Map;
    public GameObject loader;
    //GameObject currentChunk;
    //GameObject prevChunk;
    public int loadRadius = 2;
    //GameObject[] loaded;
    //string path;

    void Start()
    {
        Game = GetComponent<Game>();
        World = transform.FindChild("World").GetComponent<World>();
        Map = World.GetComponent<Map>();
        loadedChunks = new List<Chunk>();
    }

    List<Chunk> loadedChunks;
    List<Chunk> prevLoadedChunks;
    public void ChunkChanged(Chunk chunk)
    {
        int radiusCounter = 1;
        int _x = (int)chunk.coords.x;
        int _y = (int)chunk.coords.y;
        prevLoadedChunks = loadedChunks;
        loadedChunks = new List<Chunk>();

        LoadChunk(_x, _y);
        LoadChunk(_x, _y+1);
        LoadChunk(_x+1, _y);
        LoadChunk(_x+1, _y-1);
        LoadChunk(_x, _y-1);
        LoadChunk(_x-1, _y);
        LoadChunk(_x-1, _y+1);

        UnloadChunks();
    }

    Chunk LoadChunk(int x, int y)
    {
        Chunk chunk;
        if (Map.chunkMap.ContainsKey(new Vector2(x, y)))
        {
            // Chunk already exists. 
            chunk = Map.chunkMap[new Vector2(x, y)];
        }
        else
        {
            FileInfo file = FindJSONChunk(x, y);
            if (file != null)
            {
                chunk = JSONToChunk(file);
            }
            else
            {
                chunk = World.FillChunkRandom(World.CreateChunk(x, y));
            }
        }
        loadedChunks.Add(chunk);
        Debug.Log("Add " + chunk);
        return chunk;
    }

    void UnloadChunks()
    {
        List<Chunk> toUnload = new List<Chunk>();
        foreach (Chunk loaded in prevLoadedChunks)
        {
            if (!loadedChunks.Contains(loaded))
            {
                Debug.Log("Remove " + loaded);
                Map.chunkMap.Remove(loaded.coords);
                Destroy(loaded.gameObject);
            }
            else
            {

            }
        }
        //prevLoadedChunks = new List<Chunk>();
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

        string filePath = path + "/" + chunk.GetComponent<Chunk>().coords.x + "_" + chunk.GetComponent<Chunk>().coords.y + ".json";
        if (File.Exists(filePath))
            File.Delete(filePath);

        File.WriteAllText(filePath, json);
    }

    public Chunk JSONToChunk(FileInfo jsonFile)
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
        return chunk;
    }

    public FileInfo FindJSONChunk(int x, int y)
    {
        string path = Game.worldPath + "/Chunks";
        string fileName = x + "_" + y + ".json";

        if(File.Exists(path + "/" + fileName))
        {
            Debug.Log("File Exists");
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo fileInfo = dir.GetFiles(fileName)[0];
            return fileInfo;
        }
        else
        {
            Debug.Log("Does not exist");
            return null;
        }
    }
}
