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

    public World world;
    public GameObject canvas;

    //Events
    public delegate void UIOpen();
    public static event UIOpen OnUIOpen;
    public delegate void UIClose();
    public static event UIClose OnUIClose;

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
        player.GetComponent<PlayerController>().EnableMovement();

        ChunkManagement.OnChunksManaged -= SpawnPlayer;
    }

    public void OpenUI()
    {
        if (OnUIOpen != null)
            OnUIOpen();
    }
    public void CloseUI()
    {
        if (OnUIClose != null)
            OnUIClose();
    }

    public void StartCoroutinePasser(IEnumerator func)
    {
        StartCoroutine(func);
    }
}

public class GameConfig
{
    public bool DEBUG;
    public float hexSize;
    public int chunkSize;
    public int renderDistance;
}
