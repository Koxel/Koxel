using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Koxel;
using SimplexNoise;

public class World : MonoBehaviour {

    public static World instance;
    HexData hexData;
    public Dictionary<Vector3, Chunk> chunks;
    Simplex simplex;
    HexCalc hexCalc;

    private void Awake()
    {
        instance = this;

        hexData = new HexData(Game.instance.gameConfig.hexSize);
        chunks = new Dictionary<Vector3, Chunk>();
        
        hexCalc = new HexCalc();
        
        simplex = new Simplex(seed);
    }

    public Chunk AddChunk(ChunkData chunkData)
    {
        Vector3 coords = new Vector3(chunkData.coords[0], chunkData.coords[1], chunkData.coords[2]);
        Vector3 pos = new Vector3(coords.x * Game.instance.gameConfig.chunkSize * hexData.Width() + coords.y * (Game.instance.gameConfig.chunkSize / 2f * hexData.Width()), 0, coords.y * Game.instance.gameConfig.chunkSize * (.75f * hexData.Height()));
        GameObject chunkGO = ObjectPooler.instance.GetPooledObject("Chunk");
        chunkGO.transform.parent = transform;
        chunkGO.transform.localPosition = pos;
        chunkGO.name = "Chunk (" + coords.x + ", " + coords.y + ")";
        chunkGO.SetActive(true);
        Chunk chunk = chunkGO.GetComponent<Chunk>();
        chunk.coords = coords;
        if (chunkData.tiles == null)
            chunk.Generate();
        else
            chunk.Generate(chunkData.tiles);
        chunks.Add(coords, chunk);
        SaveManager.instance.SaveChunk(chunk);
        return chunk;
    }
    public Chunk AddChunk(Vector3 coords)
    {
        Vector3 pos = new Vector3(coords.x * Game.instance.gameConfig.chunkSize * hexData.Width() + coords.y * (Game.instance.gameConfig.chunkSize / 2f * hexData.Width()), 0, coords.y * Game.instance.gameConfig.chunkSize * (.75f * hexData.Height()));
        GameObject chunkGO = ObjectPooler.instance.GetPooledObject("Chunk");
        chunkGO.transform.parent = transform;
        chunkGO.transform.localPosition = pos;
        chunkGO.name = "Chunk (" + coords.x + ", " + coords.y + ")";
        chunkGO.SetActive(true);
        Chunk chunk = chunkGO.GetComponent<Chunk>();
        chunk.coords = coords;
        //chunk.Generate();
        chunks.Add(coords, chunk);

        SaveManager.instance.SaveChunk(chunk);
        return chunk;
    }

    public void RemoveChunk(Chunk chunk)
    {
        chunks.Remove(chunk.coords);
        ObjectPooler.instance.PoolObject(chunk.gameObject);
    }



    [Header("Height Noise")]
    public Color water;
    public float waterThreshold = -0f;
    public Color grass;
    public float grassThreshold = .9f;
    public Color stone;
    public long seed = 13;

    //Experiment with this Koko! :P
    public float HeightMap2(Tile tile)
    {
        Vector2 offsetCoords = hexCalc.CubeToOddR(tile.coords);
        int x = (int)offsetCoords.x;
        int y = (int)offsetCoords.y;
        x = (int)tile.coords.x;
        y = (int)tile.coords.y;
        double noise = 0;

        noise += simplex.Evaluate(x / 5f, y / 5f) / 10;
        noise += simplex.Evaluate(x / 25f, y / 25f) / 2;
        noise += simplex.Evaluate(x / 50f, y / 50f);
        noise += simplex.Evaluate(x / 100f, y / 100f);

        if (noise * 50f <= waterThreshold)
            noise = waterThreshold / 50f - 0.01f;
        return (float)noise * 50f;
    }

    //Johny's Perlin noise example
    public float HeightMap(int tileX, int tileY)
    {
        float height = 0;

        float x = (tileX);
        float y = (tileY);

        float noise100 = Mathf.PerlinNoise(x / 100.0F, y / 100.0F);
        float noise1000 = Mathf.PerlinNoise(x / 1000.0F, y / 1000.0F);
        float noise10000 = Mathf.PerlinNoise(x / 10000.0F, y / 10000.0F);

        float ground = noise100 * 0.5F + noise1000 * 0.3F + noise10000 * 0.2F;


        float noiseM1 = Mathf.PerlinNoise(0.25F + x / 80.0F, 0.25F + y / 80.0F);
        float noiseM2 = Mathf.PerlinNoise(0.25F + x / 80.0F, 0.25F + y / 80.0F);
        float noiseM = Mathf.Sqrt(noiseM1) * Mathf.Sqrt(noiseM2);
        noiseM *= 2;
        float mountain = 0;
        if (noiseM > 0.4)
        {
            mountain = (noiseM - 0.4f) * 2f;
        }

        float noiseN10 = Mathf.PerlinNoise(x / 5.0F, y / 5.0F);
        float noiseN = Mathf.PerlinNoise(0.25F + x / 25.0F, 0.25F + y / 25.0F);
        float noise = 0;
        if (noiseN > 0.55)
        {
            noise = Mathf.Max(0, noiseN10 - 0.5f) * 0.1f;
        }


        float noiseF = Mathf.PerlinNoise(0.85F + x / 40.0F, 0.85F + y / 40.0F);
        float ratioNoise = 1;
        if (noiseF > 0.5)
        {
            ratioNoise = 1 - Mathf.Max(0, (noiseF - 0.5f) * 0.5f);
        }

        height = ratioNoise * (ground + mountain) + noise;

        height *= heightMultiplier;
        //Debug.Log(height);
        return height;
    }
    public float heightMultiplier = 50f;
}
