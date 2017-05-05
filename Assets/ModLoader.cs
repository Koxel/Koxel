using System.Collections.Generic;
using UnityEngine;
using NiceJson;
using System.IO;

public class ModLoader : MonoBehaviour {
    public bool DEBUG = false;
    Dictionary<string, TileType> TileTypes;
    public Dictionary<string, Biome> Biomes;

    public void Parse()
    {
        string folderPath = Application.dataPath + "/Mods";

        TileTypes = new Dictionary<string, TileType>();
        Biomes = new Dictionary<string, Biome>();

        foreach (string mod in Directory.GetDirectories(folderPath))
        {
            string modFolder = mod.Replace(@"\", "/");
            if(DEBUG) Debug.Log(modFolder);
            DirectoryInfo tileDir = new DirectoryInfo(modFolder  + "/TileTypes");
            FileInfo[] tileInfo = tileDir.GetFiles("*.json");

            foreach (FileInfo file in tileInfo)
            {
                JsonObject array = (JsonObject)JsonNode.ParseJsonString(File.ReadAllText(file.FullName));
                CreateTileType(array);
            }
            
            DirectoryInfo biomeDir = new DirectoryInfo(modFolder + "/Biomes");
            FileInfo[] biomeInfo = biomeDir.GetFiles("*.json");

            foreach (FileInfo file in biomeInfo)
            {
                JsonObject array = (JsonObject)JsonNode.ParseJsonString(File.ReadAllText(file.FullName));
                CreateBiome(array);
            }
        }
    }

    void CreateTileType(JsonObject json)
    {
        string Hname = json["Hname"];
        string name = json["name"];
        Color defaultColor = new Color(json["defaultColor"][0], json["defaultColor"][1], json["defaultColor"][2], json["defaultColor"][3]);
        Color hoverColor = new Color(json["hoverColor"][0], json["hoverColor"][1], json["hoverColor"][2], json["hoverColor"][3]);
        float moveCost = json["stepCost"];

        TileType tileType = new TileType(Hname, name, defaultColor, hoverColor, moveCost);
        if (TileTypes.ContainsKey(tileType.Hname) || TileTypes.ContainsValue(tileType))
            Debug.LogError("TileType: " + tileType.Hname + " already exists.");
        else
        {
            if (DEBUG) Debug.Log("New TileType: " + tileType.name + " as " + tileType.Hname);
            TileTypes.Add(Hname, tileType);
        }
    }

    void CreateBiome(JsonObject json)
    {
        string Hname = json["Hname"];
        string name = json["name"];
        Dictionary<string, TileType> tileTypes = new Dictionary<string, TileType>();

        tileTypes.Add(json["defaultTile"], TileTypes[json["defaultTile"]]);

        Dictionary<string, float> multipliers = new Dictionary<string, float>();
        multipliers.Add("steps", json["multipliers"]["steps"]);
        multipliers.Add("xp", json["multipliers"]["xp_gain"]);
        multipliers.Add("damage", json["multipliers"]["damage"]);

        Biome biome = new Biome(Hname, name, tileTypes, multipliers);
        if (Biomes.ContainsKey(biome.Hname) || Biomes.ContainsValue(biome))
            Debug.LogError("TileType: " + biome.Hname + " already exists.");
        else
        {
            if (DEBUG) Debug.Log("New biome: " + biome.name + " as " + biome.Hname);
            Biomes.Add(Hname, biome);
        }
    }
}
