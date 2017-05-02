using UnityEngine;
using NiceJson;
using System.IO;

public class Game : MonoBehaviour {

    public string worldPath;
    public GameObject World;
    public GameObject Player;
    public bool menuOpen;
    WorldData worldData;

    void Start()
    {
        World = transform.FindChild("World").gameObject;

        //Load();
    }

    public void Play(string path, bool isNew, WorldData worldData)
    {
        this.worldData = worldData;
        worldPath = path;
        Load(isNew);
    }

    public void ExitGame()
    {
        Save();
        Application.Quit();
    }

    public void Save()
    {
        Debug.Log("Save");
        LoadUnload saver = GetComponent<LoadUnload>();
        for (int childNr = 0; childNr < World.transform.childCount; childNr++)
        {
            Transform child = World.transform.GetChild(childNr);
            saver.ChunkToJSON(child.gameObject, worldPath + "/Chunks");
        }
    }

    public void Load(bool isNew)
    {
        LoadUnload loader = GetComponent<LoadUnload>();

        GetComponent<ModLoader>().Parse();
        World.GetComponent<World>().SetupWorld(worldData, GetComponent<ModLoader>().Biomes);
        if (isNew)
        {
            //Generate new
            World world = World.GetComponent<World>();
            Chunk startChunk = world.FillChunkRandom(world.CreateChunk(0, 0));
            World.GetComponent<Map>().currentTile = World.GetComponent<Map>().tileMap[new Vector2(0, 0)];
            loader.ChunkChanged(startChunk);
        }
        else
        {
            //Restore save
            DirectoryInfo dir = new DirectoryInfo(worldPath + "/Chunks");
            FileInfo[] files = dir.GetFiles("*.json");
            foreach(FileInfo file in files)
            {
                loader.JSONToChunk(file);
            }
        }
        Player = World.GetComponent<World>().CreatePlayer();
        World.GetComponent<Map>().player = Player;
        GetComponent<CameraController>().Setup(Player);
    }
}
