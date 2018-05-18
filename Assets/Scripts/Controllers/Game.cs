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
    public GameObject playerUIPrefab;
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

        OnUIOpen += Pause;
        OnUIClose += Resume;

        ChunkManagement.instance.loader = Camera.main.transform.parent;
        ChunkManagement.instance.ManageChunks();
        Debug.Log("World Loaded");
    }

    private void SpawnPlayer(Chunk originChunk)
    { 
        //Create Player
        GameObject player = Instantiate(playerPrefab, originChunk.tiles[new Vector3(0,0,0)].transform.position, Quaternion.identity);
        player.name = player.name.Replace("(Clone)", "");

        //Create UI
        GameObject playerUI = Instantiate(playerUIPrefab, canvas.transform);
        PlayerUIController controller = playerUI.GetComponent<PlayerUIController>();
        controller.player = player.GetComponent<Player>();

        player.GetComponent<Player>().UIcontroller = controller;
        player.GetComponent<PlayerController>().EnableMovement();

        ChunkManagement.instance.loader = player.transform;
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

    public static void Pause()
    {
        Time.timeScale = 0f;
    }
    public static void Resume()
    {
        Time.timeScale = 1f;
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
