using System.Collections.Generic;
using UnityEngine;

public class Biome {
    public string Hname;
    public string name;
    public Color skyColor;
    public Color worldColor;
    public Dictionary<string, float> multipliers;
    public Dictionary<string, TileType> tileTypes;
    public List<TileAsset> tileAssets;

    public Biome(string Hname, string name, Dictionary<string, TileType> tileTypes, Dictionary<string, float> multipliers)
    {
        this.Hname = Hname;
        this.name = name;
        this.tileTypes = tileTypes;
        this.multipliers = multipliers;
    }
}