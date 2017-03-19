using System.Collections.Generic;
using UnityEngine;

public class Biome {
    public string name;
    public TileType defaultTileType;
    public Color skyColor;
    public Color worldColor;
    public Dictionary<string, float> multipliers;
    public List<BiomeTile> extraTiles;
    public List<BiomeTileAsset> tileAssets;

    public Biome(string name, TileType defaultTileType, Dictionary<string, float> multipliers)
    {

    }
}

public class BiomeTile
{
    public TileType tile;
    public double rarity;
    public int minHeight;
    public int maxHeight;
    public int minPatchSize;
    public int maxPatchSize;
}


public class BiomeTileAsset
{
    public TileAsset asset;
    public double rarity;
    public int minHeight;
    public int maxHeight;
    public List<TileType> spawnTiles;
}

