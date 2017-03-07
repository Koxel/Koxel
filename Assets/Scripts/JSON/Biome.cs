using System.Collections.Generic;
using UnityEngine;

public class Biome {
    public string name;
    public string defaultTile;
    public Color skyColor;
    public Color worldColor;
    public Dictionary<string, double> multipliers;
    public List<BiomeTile> extraTiles;
    public List<BiomeTileAsset> tileAssets;
}

public class BiomeTile
{
    public Tile tile;
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
    public List<Tile> spawnTiles;
}

