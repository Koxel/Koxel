using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using Koxel;

public class ChunkData
{
    public int[] coords;
    public List<TileData> tiles;

    public ChunkData(Vector3 coords)
    {
        this.coords = new int[3];
        this.coords[0] = (int)coords.x;
        this.coords[1] = (int)coords.y;
        this.coords[2] = (int)coords.z;
    }
    public ChunkData(Chunk chunk)
    {
        this.coords = new int[3];
        this.coords[0] = (int)chunk.coords.x;
        this.coords[1] = (int)chunk.coords.y;
        this.coords[2] = (int)chunk.coords.z;
        this.tiles = new List<TileData>();
        foreach (Tile tile in chunk.tiles.Values)
        {
            this.tiles.Add(new TileData(tile));
        }
    }
    public ChunkData(int[] coords, List<TileData> tiles)
    {
        this.coords = coords;
        this.tiles = tiles;
    }
    public ChunkData()
    {
        this.tiles = new List<TileData>(); ;
        this.coords = new int[3];
    }
}

public class TileData
{
    public string biome;
    public string tileType;

    public TileData(Tile tile)
    {
        if (tile != null)
        {
            biome = tile.biome;
            tileType = tile.tileType;
        }
    }
}

public class SaveManager : MonoBehaviour {

    public static SaveManager instance;

    public SaveData save;
    string worldPath;

	void Awake () {
        instance = this;

        if (!Directory.Exists(Directory.GetParent(Application.dataPath).FullName + "/Saves"))
            Directory.CreateDirectory(Directory.GetParent(Application.dataPath).FullName + "/Saves");

        NewSave("MyWorld", 0);
	}

    private void Start()
    {
        //SaveChunk(new Vector3(0, 0, 0));
    }

    public void NewSave(string name, float seed)
    {
        string creationTime = (DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString()).Replace("/", "-").Replace(":", "_").Replace(" ", "_");
        string saveFolder = Directory.GetParent(Application.dataPath).FullName + "/Saves/" + creationTime;
        if (Directory.Exists(saveFolder))
        {
            saveFolder = saveFolder + "_" + DateTime.Now.ToShortTimeString();
            saveFolder.Replace("/", "-").Replace(":", "_").Replace(" ", "_");
        }
        Directory.CreateDirectory(saveFolder);
        Directory.CreateDirectory(saveFolder + "/Chunks");
        SaveData sd = new SaveData()
        {
            worldName = name,
            seed = seed,
            creationTime = creationTime
        };
        string jsonFile = JsonConvert.SerializeObject(sd, Formatting.Indented);
        File.WriteAllText(saveFolder + "/save_data.json", jsonFile);
        save = sd;
        worldPath = Directory.GetParent(Application.dataPath).FullName + "/Saves/" + save.creationTime;
    }

    public void LoadSave(SaveData savedata)
    {
        save = savedata;
        worldPath = Directory.GetParent(Application.dataPath).FullName + "/Saves/" + save.creationTime;
    }
    public void LoadSave(string filePath)
    {
        save = JsonConvert.DeserializeObject<SaveData>(filePath);
    }

    public void SaveChunk(Chunk chunk)
    {
        ChunkData cd = new ChunkData(chunk);
        string jsonFile = JsonConvert.SerializeObject(cd, Formatting.Indented);
        File.WriteAllText(worldPath + "/Chunks/" + chunk.coords.x + ", " + chunk.coords.y + ".json", jsonFile);
    }

    public ChunkData LoadChunk(Vector3 coords)
    {
        Debug.Log(worldPath + "/Chunks/" + coords.x + ", " + coords.y + ".json");
        if(File.Exists(worldPath + "/Chunks/" + coords.x + ", " + coords.y + ".json")) {
            ChunkData chunkdata = JsonConvert.DeserializeObject<ChunkData>(File.ReadAllText(worldPath + "/Chunks/" + coords.x + ", " + coords.y + ".json"));
            return chunkdata;
        }
        else
        {
            Debug.LogError("Chunk has not been saved yet.");
            return null;
        }
    }

    public bool IsChunkSaved(Vector3 coords)
    {
        string path = worldPath + "/Chunks";
        string fileName = coords.x + "_" + coords.y + ".json";

        if (File.Exists(path + "/" + fileName))
        {
            //Debug.Log("File Exists");
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo fileInfo = dir.GetFiles(fileName)[0];
            return true;
        }
        else
        {
            //Debug.Log("Does not exist");
            return false;
        }
    }
}

public class SaveData
{
    public string worldName;
    public float seed;
    public string creationTime;
}