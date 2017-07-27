using Koxel;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Chunk : MonoBehaviour {

    bool hasGenerated;
    public GameObject tilePrefab;
    public Vector3 coords;
    public Dictionary<Vector3, Tile> tiles = new Dictionary<Vector3, Tile>();

    [Header("Debug")]
    public List<Vector3> tileCoordsDEBUG;
    public List<Tile> tileDEBUG;

    void Awake()
    {
        //Generate();
        
        if (!hasGenerated)
        {
            tileCoordsDEBUG = new List<Vector3>();
            tileDEBUG = new List<Tile>();
            GenerateNew();
        }
    }

    private void Start()
    {
        
    }

    public void Generate()
    {
        //Debug.Log("Generate");
        tileDEBUG.AddRange(tiles.Values);
        tileCoordsDEBUG.AddRange(tiles.Keys);

        HexData hexData = new HexData(Game.instance.gameConfig.hexSize);
        
        for (int r = -Game.instance.gameConfig.chunkSize / 2; r <= Game.instance.gameConfig.chunkSize / 2; r++)
        {
            for (int q = -Game.instance.gameConfig.chunkSize / 2; q <= Game.instance.gameConfig.chunkSize / 2; q++)
            {
                GameObject tileGO = tiles[new Vector3(r, q, -r - q)].gameObject;
                int xr = (int)(r + Game.instance.gameConfig.chunkSize * coords.x);
                int yq = (int)(q + Game.instance.gameConfig.chunkSize * coords.y);
                Vector3 pos = new Vector3(
                    tileGO.transform.localPosition.x,0,
                    //World.instance.HeightMap(xr, yq),
                    tileGO.transform.localPosition.z
                );
                tileGO.transform.localPosition = pos;
                tileGO.name = "Tile (" + xr + ", " + yq + ")";
                Tile tile = tileGO.GetComponent<Tile>();
                tile.coords = new Vector3(xr, yq, -xr - yq);
                tile.chunk = this;
                tile.biome = "randomBiome";
                tile.tileType = "randomTileType";
                float perlin = World.instance.HeightMap2((int)(tile.coords.x), (int)(tile.coords.y));
                Vector3 posy = tile.transform.position;
                posy.y = perlin;
                tile.transform.position = posy;

                if (perlin < 0f * World.instance.heightScale)
                {
                    tile.SetColor(World.instance.water);
                }
                else if (perlin < .9f * World.instance.heightScale)
                {
                    tile.SetColor(World.instance.grass);
                }
                else
                {
                    tile.SetColor(World.instance.stone);
                }
                //World.instance.HeightMap2(xr, yq);
            }
        }
    }
    public void Generate(List<TileData> tileDatas)
    {
        //Debug.Log("Generate with tileDatas");
        HexData hexData = new HexData(Game.instance.gameConfig.hexSize);

        //tiles = new Dictionary<Vector3, Tile>();
        for (int r = -Game.instance.gameConfig.chunkSize / 2; r <= Game.instance.gameConfig.chunkSize / 2; r++)
        {
            for (int q = -Game.instance.gameConfig.chunkSize / 2; q <= Game.instance.gameConfig.chunkSize / 2; q++)
            {
                GameObject tileGO = tiles[new Vector3(r, q, -r - q)].gameObject;
                Vector3 pos = new Vector3(
                    tileGO.transform.localPosition.x,
                    World.instance.HeightMap(r, q) * 10f,
                    tileGO.transform.localPosition.z
                );
                tileGO.transform.localPosition = pos;
                tileGO.transform.localScale = tileGO.transform.localScale * hexData.Size();
                tileGO.name = "Tile (" + q + ", " + r + ")";
                Tile tile = tileGO.GetComponent<Tile>();
                tile.coords = new Vector3(r, q, -r - q) + this.coords;
                tile.chunk = this;
                tile.biome = tileDatas[0].biome;
                tile.tileType = tileDatas[0].tileType;
                tileDatas.RemoveAt(0);
                tile.SetColor(new Color(0, .5f, 0, .5f));
                //tiles.Add(tile.coords, tile);
            }
        }
    }

    public void GenerateNew()
    {
        //Debug.Log("Generate new");
        HexData hexData = new HexData(Game.instance.gameConfig.hexSize);

        tiles = new Dictionary<Vector3, Tile>();
        for (int r = -Game.instance.gameConfig.chunkSize / 2; r <= Game.instance.gameConfig.chunkSize / 2; r++)
        {
            for (int q = -Game.instance.gameConfig.chunkSize / 2; q <= Game.instance.gameConfig.chunkSize / 2; q++)
            {
                Vector3 pos = new Vector3(
                    r * hexData.Width() + q * (.5f * hexData.Width()),
                    0,
                    q * (hexData.Height() * .75f)
                );
                GameObject tileGO = Instantiate(tilePrefab, transform);
                tileGO.transform.localPosition = pos;
                tileGO.transform.localScale = tileGO.transform.localScale * hexData.Size();
                tileGO.name = "Tile (" + q + ", " + r + ")";
                Tile tile = tileGO.GetComponent<Tile>();
                tile.coords = new Vector3(r, q, -r - q) + this.coords;
                tile.chunk = this;
                tile.biome = "randomBiome";
                tile.tileType = "randomTileType";
                tile.SetColor(new Color(0,.5f,0,.5f));
                tiles.Add(new Vector3(r,q,-r-q), tile);
            }
        }
        hasGenerated = true;
    }
    public void GenerateNew(List<TileData> tileDatas)
    {
        HexData hexData = new HexData(Game.instance.gameConfig.hexSize);

        tiles = new Dictionary<Vector3, Tile>();
        for (int r = -Game.instance.gameConfig.chunkSize / 2; r <= Game.instance.gameConfig.chunkSize / 2; r++)
        {
            for (int q = -Game.instance.gameConfig.chunkSize / 2; q <= Game.instance.gameConfig.chunkSize / 2; q++)
            {
                Vector3 pos = new Vector3(
                    r * hexData.Width() + q * (.5f * hexData.Width()),
                    0,
                    q * (hexData.Height() * .75f)
                );
                GameObject tileGO = Instantiate(tilePrefab, transform);
                tileGO.transform.localPosition = pos;
                tileGO.transform.localScale = tileGO.transform.localScale * hexData.Size();
                tileGO.name = "Tile (" + q + ", " + r + ")";
                Tile tile = tileGO.GetComponent<Tile>();
                tile.coords = new Vector3(r, q, -r - q) + this.coords;
                tile.chunk = this;
                tile.biome = tileDatas[0].biome;
                tile.tileType = tileDatas[0].tileType;
                tileDatas.RemoveAt(0);
                tile.SetColor(new Color(0, .5f, 0, .5f));
                tiles.Add(new Vector3(r, q, -r - q), tile);
            }
        }
        hasGenerated = true;
    }
}
