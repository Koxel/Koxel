using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class JsonLoader : MonoBehaviour {

    /*public Dictionary<string, TileType> Tiles = new Dictionary<string, TileType>();
    public List<Biome> Biomes = new List<Biome>();

    // Use this for initialization
    void Start () {
        // TileTypes
        DirectoryInfo tileDir = new DirectoryInfo(Application.dataPath + "/Resources/Tiles");
        FileInfo[] tileInfo = tileDir.GetFiles("*.json");
        
        foreach (FileInfo file in tileInfo)
        {
            CreateTile(JsonMapper.ToObject(File.ReadAllText(file.FullName)));
        }

        // Biomes
        DirectoryInfo biomeDir = new DirectoryInfo(Application.dataPath + "/Resources/Biomes");
        FileInfo[] biomeInfo = biomeDir.GetFiles("*.json");

        foreach (FileInfo file in biomeInfo)
        {
            CreateBiome(JsonMapper.ToObject(File.ReadAllText(file.FullName)));
        }

        // We're ready, start Generation
        GetComponent<WorldGenerator>().Generate(null, Biomes);
    }

    void CreateTile(JsonData jsonData)
    {
        string tileName = jsonData["name"].ToString();
        Color tileDefaultColor = new Color((int)jsonData["defaultRGB"][0] / 255, (int)jsonData["defaultRGB"][1] / 255, (int)jsonData["defaultRGB"][2] / 255, (int)jsonData["defaultRGB"][3] / 255);
        Color tileHoverColor = new Color((int)jsonData["hoverRGB"][0] / 255, (int)jsonData["hoverRGB"][1] / 255, (int)jsonData["hoverRGB"][2] / 255, (int)jsonData["hoverRGB"][3] / 255);
        Debug.Log(tileName + ": " + (int)jsonData["defaultRGB"][0]);
        float moveCost = (int)jsonData["moveCost"];

        Tiles.Add(tileName, new TileType(tileName, tileDefaultColor, tileHoverColor, moveCost));
    }

    void CreateBiome(JsonData jsonData)
    {
        string biomeName = jsonData["name"].ToString();
        TileType tileType = Tiles[jsonData["default_tile"].ToString()];
        Dictionary<string, float> multipliers = new Dictionary<string, float>();
        foreach (string key in jsonData["multipliers"].Keys)
        {
            float value = (int)jsonData["multipliers"][key];
            multipliers.Add(key, value/100);
        }
        
        Biomes.Add(new Biome(biomeName, tileType, multipliers));
    }*/
}
