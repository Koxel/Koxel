using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiceJson;
using System.IO;

public class NiceJsonLoader : MonoBehaviour {
    public bool DEBUG = false;
    Dictionary<string, TileType> Tiles;
    Dictionary<string, Biome> Biomes;

    // Use this for initialization
    void Start () {
        Tiles = new Dictionary<string, TileType>();
        DirectoryInfo tileDir = new DirectoryInfo(Application.dataPath + "/Mods/Koxel/Tiles");
        FileInfo[] tileInfo = tileDir.GetFiles("*.json");

        foreach (FileInfo file in tileInfo)
        {
            JsonObject array = (JsonObject)JsonNode.ParseJsonString(File.ReadAllText(file.FullName));
            CreateTile(array);
        }
        if (DEBUG)
        {
            foreach (string text in Tiles.Keys)
            {
                Debug.Log(text);
            }
        }

        Biomes = new Dictionary<string, Biome>();
        DirectoryInfo biomeDir = new DirectoryInfo(Application.dataPath + "/Mods/Koxel/Biomes");
        FileInfo[] biomeInfo = biomeDir.GetFiles("*.json");

        foreach (FileInfo file in biomeInfo)
        {
            JsonObject array = (JsonObject)JsonNode.ParseJsonString(File.ReadAllText(file.FullName));
            CreateBiome(array);
        }
        if (DEBUG)
        {
            foreach (string text in Biomes.Keys)
            {
                Debug.Log(text);
            }
        }

        // JSON DONE start generation
        GetComponent<WorldGenerator>().Generate(Biomes);
    }

    void CreateTile(JsonObject json)
    {
        string Hname = json["Hname"];
        string name = json["name"];
        Color defaultColor = new Color(json["defaultColor"][0], json["defaultColor"][1], json["defaultColor"][2], json["defaultColor"][3]);
        Color hoverColor = new Color(json["hoverColor"][0], json["hoverColor"][1], json["hoverColor"][2], json["hoverColor"][3]);
        float moveCost = json["stepCost"];

        TileType tileType = new TileType(name, defaultColor, hoverColor, moveCost);
        Tiles.Add(Hname, tileType);
    }

    void CreateBiome(JsonObject json)
    {
        string Hname = json["Hname"];
        string name = json["name"];
        List<TileType> tiles = new List<TileType>();
        tiles.Add(Tiles[json["defaultTile"]]);
        //Debug.Log(json["tiles"][0]["Hname"]);
        //tiles.Add(Tiles[json["tiles"]["Forest Tile"]["Hname"]]);

        Dictionary<string, float> multipliers = new Dictionary<string, float>();
        multipliers.Add("steps", json["multipliers"]["steps"]);
        multipliers.Add("xp", json["multipliers"]["xp_gain"]);
        multipliers.Add("damage", json["multipliers"]["damage"]);
        /*foreach (string key in json["multipliers"])
        {
            float value = (int)json["multipliers"][key];
            multipliers.Add(key, value / 100);
        }*/

        Biome biome = new Biome(name, tiles, multipliers);
        Biomes.Add(Hname, biome);
    }
}
