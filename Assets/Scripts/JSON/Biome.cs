using System.Collections.Generic;
using UnityEngine;

public class Biome {
    public string name;
    public Color skyColor;
    public Color worldColor;
    public Dictionary<string, float> multipliers;
    public List<TileType> tiles;
    public List<TileAsset> tileAssets;

    public Biome(string name, List<TileType> tiles, Dictionary<string, float> multipliers)
    {
        this.name = name;
        this.tiles = tiles;
        this.multipliers = multipliers;
    }
}