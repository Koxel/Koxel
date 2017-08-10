using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Koxel;

public class Game : MonoBehaviour {
    public static Game instance;

    public GameConfig gameConfig;
    public GameObject tilePrefab;
    public static HexData hexData;
    World world;

	void Awake () {
        instance = this;

        gameConfig = JsonConvert.DeserializeObject<GameConfig>(File.ReadAllText(Directory.GetParent(Application.dataPath).FullName + "/Game/GameConfig.json").Replace(@"\", "/"));
        hexData = new HexData(Game.instance.gameConfig.hexSize);
        world = GameObject.Find("World").GetComponent<World>();
    }
	
}

public class GameConfig
{
    public bool DEBUG;
    public float hexSize;
    public int chunkSize;
    public int renderDistance;
}
