using UnityEngine;
using NiceJson;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class LoadUnload : MonoBehaviour {

    Game Game;
    World World;
    Map Map;
    public GameObject loader;
    public int loadRadius = 5;

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
        int _x = (int)chunk.coords.x;
        int _y = (int)chunk.coords.y;
        prevLoadedChunks = loadedChunks;
        loadedChunks = new List<Chunk>();

        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for(int y = Mathf.Max(-loadRadius, -x-loadRadius); y <= Mathf.Min(loadRadius, -x+loadRadius); y++)
            {
                LoadChunk(_x+x, _y+y);
            }
        }
        UnloadChunks();
    }

    void LoadChunk(int x, int y)
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
                chunk = World.PooledChunk(x, y, ObjectPooler.SharedInstance.GetPooledObject("Chunk"));
                for(int i = 0; i < chunk.transform.childCount; i++)
                {
                    Tile tile = chunk.transform.GetChild(i).GetComponent<Tile>();
                    World.PooledTile(tile, (int)tile.chunkCoords.x, (int)tile.chunkCoords.y, chunk, null, null);
                }
                //chunk = World.FillChunkRandom(World.CreateChunk(x, y));
            }
        }
        loadedChunks.Add(chunk);
    }

    void UnloadChunks()
    {
        foreach (Chunk loaded in prevLoadedChunks)
        {
            if (!loadedChunks.Contains(loaded))
            {
                Debug.Log("Unload");
                RemoveChunk(loaded);
            }
        }
    }

    void RemoveChunk(Chunk chunk)
    {
        ChunkToJSON(chunk.gameObject, Game.worldPath + "/Chunks");
        foreach (Tile tile in chunk.tiles.Values)
        {
            Map.tileMap.Remove(tile.worldCoords);
            //ObjectPooler.SharedInstance.PoolObject(tile.gameObject);
        }
        Map.chunkMap.Remove(chunk.coords);
        ObjectPooler.SharedInstance.PoolObject(chunk.gameObject);
    }
    

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
        //Chunk chunk = World.CreateChunk(json["coords"][0], json["coords"][1]);
        Chunk chunk = World.PooledChunk(json["coords"][0], json["coords"][1], ObjectPooler.SharedInstance.GetPooledObject("Chunk"));
        int tileCount = 0;
        for (int r = -World.chunkRadius / 2; r < World.chunkRadius / 2; r++)
        {
            int r_offset = (int)Mathf.Floor(r / 2);
            for (int q = -World.chunkRadius / 2 - r_offset; q < World.chunkRadius / 2 - r_offset; q++)
            {
                //x=r, y=q
                Biome biome = World.FindBiome(json["tiles"][tileCount][0]);
                TileType tileType = biome.tileTypes[json["tiles"][tileCount][1]];
                Tile tile = chunk.tiles[new Vector2(r, q)];
                World.PooledTile(tile, r, q, chunk, biome, tileType);
                //World.CreateTile(r, q, chunk, biome, tileType);
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
            //Debug.Log("File Exists");
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo fileInfo = dir.GetFiles(fileName)[0];
            return fileInfo;
        }
        else
        {
            //Debug.Log("Does not exist");
            return null;
        }
    }
}
