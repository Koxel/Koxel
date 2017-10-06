using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Koxel;

public class Game : MonoBehaviour {
    public static Game instance;

    public GameConfig gameConfig;
    public GameObject playerPrefab;
    public GameObject worldCursorPrefab;
    public static HexData hexData;

    World world;

	void Awake () {
        instance = this;

        gameConfig = JsonConvert.DeserializeObject<GameConfig>(File.ReadAllText(Directory.GetParent(Application.dataPath).FullName + "/Game/GameConfig.json").Replace(@"\", "/"));
        hexData = new HexData(Game.instance.gameConfig.hexSize);
        world = GameObject.Find("World").GetComponent<World>();

        ChunkManagement.OnChunksManaged += SpawnPlayer;
    }

    private void Start()
    {
        ModLoader.instance.LoadMods();
        Debug.Log("Mods Loaded");
        World.instance.tileAssets.AddRange(ModLoader.TileAssets.Values);

        ChunkManagement.instance.loader = PlayerCamera.instance.transform;
        ChunkManagement.instance.ManageChunks();
        Debug.Log("World Loaded");
    }

    private void SpawnPlayer(Chunk originChunk)
    { 
        GameObject player = Instantiate(playerPrefab, originChunk.tiles[new Vector3(0,0,0)].transform.position, Quaternion.identity);
        ChunkManagement.instance.loader = player.transform;
        PlayerCamera.instance.target = player.transform;
        PlayerCamera.instance.transform.position = player.transform.position;
        player.name = player.name.Replace("(Clone)", "");

        GameObject cursor = Instantiate(worldCursorPrefab);
        cursor.name = cursor.name.Replace("(Clone)", "");

        ChunkManagement.OnChunksManaged -= SpawnPlayer;
    }
}

public class GameConfig
{
    public bool DEBUG;
    public float hexSize;
    public int chunkSize;
    public int renderDistance;
}
