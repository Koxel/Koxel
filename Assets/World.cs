using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Koxel;

public class World : MonoBehaviour {

    public static World instance;
    HexData hexData;
    public Dictionary<Vector3, Chunk> chunks;

    private void Awake()
    {
        instance = this;

        hexData = new HexData(Game.instance.gameConfig.hexSize);
        chunks = new Dictionary<Vector3, Chunk>();

        Debug.Log(Mathf.PerlinNoise(5f/10f, 2f/10f));
        Debug.Log(Mathf.PerlinNoise(9f/10f, 3f/10f));
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



    [Header("Perlin Noise")]
    public float heightScale = 5f;
    public float detailScale = 5f;

    public Color water;
    public Color grass;
    public Color stone;

public float HeightMap2(int x, int y)
{
    float noise = 1f * Mathf.PerlinNoise(x / 10f, y / 10f)
            + .5f * Mathf.PerlinNoise(x / 10f, y / 10f)
        + .25f * Mathf.PerlinNoise(x/10f, y / 10f);
    float elevation = Mathf.Pow(noise, 3);
    Debug.Log("X, Y: " + x + ", " + y + "\nNoise: " + noise + "\nElevation: " + elevation);
    return elevation * 10f;
}

    public float HeightMap(int tileX, int tileY)
    {
        float height = 0;

        float x = (tileX);
        float y = (tileY);

        float noise100 = Mathf.PerlinNoise(x / 100.0F, y / 100.0F) * heightScale;
        float noise1000 = Mathf.PerlinNoise(x / 1000.0F, y / 1000.0F) * heightScale;
        float noise10000 = Mathf.PerlinNoise(x / 10000.0F, y / 10000.0F) * heightScale;

        float ground = noise100 * 0.5F + noise1000 * 0.3F + noise10000 * 0.2F;


        float noiseM1 = Mathf.PerlinNoise(0.25F + x / 40.0F, 0.25F + y / 40.0F) * heightScale;
        float noiseM2 = Mathf.PerlinNoise(0.25F + x / 40.0F, 0.25F + y / 40.0F) * heightScale;
        float noiseM = Mathf.Sqrt(noiseM1) * Mathf.Sqrt(noiseM2);
        float mountain = 0;
        if (noiseM > 0.4)
        {
            mountain = (noiseM - 0.4f) * 0.4f;
        }

        float noiseN10 = Mathf.PerlinNoise(x / 5.0F, y / 5.0F) * heightScale;
        float noiseN = Mathf.PerlinNoise(0.25F + x / 25.0F, 0.25F + y / 25.0F) * heightScale;
        float noise = 0;
        if (noiseN > 0.55)
        {
            noise = Mathf.Max(0, noiseN10 - 0.5f) * 0.1f;
        }


        float noiseF = Mathf.PerlinNoise(0.85F + x / 40.0F, 0.85F + y / 40.0F) * heightScale;
        float ratioNoise = 1;
        if (noiseF > 0.5)
        {
            ratioNoise = 1 - Mathf.Max(0, (noiseF - 0.5f) * 0.5f);
        }

        height = ratioNoise * (ground + mountain) + noise;

        height *= heightScale;
        //Debug.Log(height);
        return height;
    }
    public float heightMultiplier = 50f;
}
