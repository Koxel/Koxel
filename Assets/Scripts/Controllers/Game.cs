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
    public GameObject loadingTextPrefab;
    public GameObject loadingText;
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

        //Show loading text
        loadingText = Instantiate(loadingTextPrefab, canvas.transform);

        ChunkManagement.instance.loader = Camera.main.transform.parent;
        ChunkManagement.instance.ManageChunks();
        Debug.Log("World Loaded");
    }

    private void SpawnPlayer(Chunk originChunk)
    {
        //Remove the loading text
        Destroy(loadingText);

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

        StartCoroutine(ShowStartPopups());
    }

    IEnumerator ShowStartPopups()
    {
        PopupManager.instance.ShowInfo("Use the WASD keys to move around and Space to jump.", 8000);
        yield return new WaitForSeconds(10000/1000);
        PopupManager.instance.ShowInfo("Walk up to an object and use your mouse to scroll through the menu, then click to select.", 9000);
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
