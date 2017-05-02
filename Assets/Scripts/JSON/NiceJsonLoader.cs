using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiceJson;
using System.IO;

public class NiceJsonLoader : MonoBehaviour {
    public bool DEBUG = false;
    Dictionary<string, TileType> TileTypes;
    public Dictionary<string, Biome> Biomes;

    public void Parse ()
    {
        TileTypes = new Dictionary<string, TileType>();
        Debug.Log(Application.dataPath);
        DirectoryInfo tileDir = new DirectoryInfo(Application.dataPath + "/Mods/Koxel/TileTypes");
        FileInfo[] tileInfo = tileDir.GetFiles("*.json");

        foreach (FileInfo file in tileInfo)
        {
            JsonObject array = (JsonObject)JsonNode.ParseJsonString(File.ReadAllText(file.FullName));
            CreateTileType(array);
        }
        if (DEBUG)
            foreach (string text in TileTypes.Keys)
                Debug.Log(text);

        Biomes = new Dictionary<string, Biome>();
        DirectoryInfo biomeDir = new DirectoryInfo(Application.dataPath + "/Mods/Koxel/Biomes");
        FileInfo[] biomeInfo = biomeDir.GetFiles("*.json");

        foreach (FileInfo file in biomeInfo)
        {
            JsonObject array = (JsonObject)JsonNode.ParseJsonString(File.ReadAllText(file.FullName));
            CreateBiome(array);
        }
        if (DEBUG)
            foreach (string text in Biomes.Keys)
                Debug.Log(text);
    }

    void CreateTileType(JsonObject json)
    {
        string Hname = json["Hname"];
        string name = json["name"];
        Color defaultColor = new Color(json["defaultColor"][0], json["defaultColor"][1], json["defaultColor"][2], json["defaultColor"][3]);
        Color hoverColor = new Color(json["hoverColor"][0], json["hoverColor"][1], json["hoverColor"][2], json["hoverColor"][3]);
        float moveCost = json["stepCost"];

        TileType tileType = new TileType(Hname, name, defaultColor, hoverColor, moveCost);
        TileTypes.Add(Hname, tileType);
    }

    void CreateBiome(JsonObject json)
    {
        string Hname = json["Hname"];
        string name = json["name"];
        Dictionary<string, TileType> tileTypes = new Dictionary<string, TileType>();
        
        tileTypes.Add(json["defaultTile"], TileTypes[json["defaultTile"]]);
        //Debug.Log(json["tiles"][0]["Hname"]);
        //tiles.Add(TileTypes[json["tiles"]["Forest Tile"]["Hname"]]);

        Dictionary<string, float> multipliers = new Dictionary<string, float>();
        multipliers.Add("steps", json["multipliers"]["steps"]);
        multipliers.Add("xp", json["multipliers"]["xp_gain"]);
        multipliers.Add("damage", json["multipliers"]["damage"]);
        /*foreach (string key in json["multipliers"])
        {
            float value = (int)json["multipliers"][key];
            multipliers.Add(key, value / 100);
        }*/

        Biome biome = new Biome(Hname, name, tileTypes, multipliers);
        Biomes.Add(Hname, biome);
    }
}
