using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChunkManagement : MonoBehaviour {

    public static ChunkManagement instance;

    World world;
    Chunk currentChunk;
    public Transform loader;

    //Procedural Terrain
    private Dictionary<Vector2, ChunkInfo> chunksStore = new Dictionary<Vector2, ChunkInfo>();
    private List<ChunkInfo> unloadChunks = new List<ChunkInfo>();
    private List<ChunkInfo> loadChunks = new List<ChunkInfo>();

    [Header("Debug")]
    public int chunksStoreCount;
    public int unloadChunksCount;
    public int loadChunksCount;

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        world = GetComponent<World>();

        //ChangeChunk(world.AddChunk(new Vector3(0,0,0)));
        ManageChunks();
	}

    private void Update()
    {
        if (loader != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(loader.position, -Vector3.up, out hit, Mathf.Infinity, 1 << 8))
            {
                if (hit.collider.GetComponentInParent<Tile>().chunk != currentChunk)
                {
                    currentChunk = hit.collider.GetComponentInParent<Tile>().chunk;
                    ThreadQueuer.instance.StartThreadedFunction(ManageChunks);
                }
            }
        }

        UpdateChunks();
    }

    public void ChangeChunk(Chunk newChunk)
    {
        currentChunk = newChunk;
        ManageChunks();
    }

    void ManageChunks()
    {
        if (currentChunk == null)
        {
            currentChunk = new Chunk()
            {
                coords = new Vector3()
            };
        }

        int N = Game.instance.gameConfig.renderDistance;
        int xmin = (int)currentChunk.coords.x - N;
        int ymin = (int)currentChunk.coords.y - N;
        int zmin = (int)currentChunk.coords.z - N;
        int xmax = (int)currentChunk.coords.x + N;
        int ymax = (int)currentChunk.coords.y + N;
        int zmax = (int)currentChunk.coords.z + N;


        List<Vector3> results = new List<Vector3>();
        for (int x = xmin; x <= xmax; x++)
        {
            for (int y = Mathf.Max(ymin, -x - zmax); y <= Mathf.Min(ymax, -x - zmin); y++)
            {
                int z = -x - y;
                results.Add(new Vector3(x, y, z));
            }
        }

        /// This code has been created by the awesome JohnyCilhokla
        /// for an old version of Koxel. I (Pixel) edited to make it work
        /// for the latest version.
        /// Thank you so much, Johny! :D ❤
        /// https://twitter.com/JohnyCilohokla

        foreach (Vector3 coords in results)
        {
            bool create = false;
            if (chunksStore.ContainsKey(coords))
            {
                ChunkInfo chunkInfo = chunksStore[coords];
                lock (unloadChunks)
                {
                    if (Monitor.TryEnter(chunkInfo, 0))
                    {
                        try
                        {
                            if (chunkInfo.chunk == null && chunkInfo.data == null || (!chunkInfo.isLoaded && !chunkInfo.isLoading))
                            {
                                create = true;
                            }
                            else if (chunkInfo.isUnloading) // chunk is queued to be unloaded
                            {
                                chunkInfo.isUnloading = false; // remove unload flag
                                unloadChunks.Remove(chunkInfo); // remove from the unload queue
                            }
                        }
                        finally
                        {
                            Monitor.Exit(chunkInfo);
                        }
                    }
                }
            }
            else
            {
                create = true;
            }
            if (create)
            {
                ChunkData data;
                /// load the chunk data here
                /// this will save the main thread from doing that
                /// you could/should also generate the chunks here and save the generated data
                /// (then just setup the tiles/create objects in the main thread)
                if (SaveManager.instance.IsChunkSaved(coords))
                {
                    data = SaveManager.instance.LoadChunk(coords);
                }
                else
                {
                    data = new ChunkData(coords);
                }

                ChunkInfo chunkInfo = new ChunkInfo(data);
                chunksStore[coords] = chunkInfo; // add to the chunk store map

                lock (loadChunks)
                {
                    chunkInfo.isLoading = true; // add load flag
                    loadChunks.Add(chunkInfo); // add to the load queue
                }
            }
        }

        List<Vector2> removals = new List<Vector2>();
        var it = chunksStore.GetEnumerator();
        while (it.MoveNext()) // loop through each chunk 
        {
            Vector2 pos = it.Current.Key;
            ChunkInfo chunkInfo = it.Current.Value;

            if (Monitor.TryEnter(chunkInfo, 0))
            {
                try
                {
                    if ((chunkInfo.isLoaded && !chunkInfo.isUnloading) || (!chunkInfo.isLoaded && chunkInfo.isLoading))
                    {
                        if (pos.x < xmin - 1 || pos.x > xmax + 1 || pos.y < Mathf.Max(ymin, -pos.x - zmax) - 1 || pos.y > Mathf.Min(ymax, -pos.x - zmin) + 1) // check if the chunk it outside of the "view"
                        {
                            if (chunkInfo.isLoaded)
                            {
                                lock (unloadChunks)
                                {
                                    chunkInfo.isUnloading = true; // add unload flag
                                    unloadChunks.Add(chunkInfo); // add to the unload queue
                                }
                            }
                            else
                            {
                                lock (loadChunks)
                                {
                                    chunkInfo.isLoading = false; // remove load flag
                                    loadChunks.Remove(chunkInfo); // remove from the load queue
                                }
                            }
                        }
                    }

                    if (chunkInfo.data == null && chunkInfo.chunk == null)
                    {
                        removals.Add(pos);
                    }
                }
                finally
                {
                    Monitor.Exit(chunkInfo);
                }
            }
        }
        foreach (Vector2 removal in removals)
        {
            chunksStore.Remove(removal);
        }

    }

    void UpdateChunks()
    {
        while (true)
        {
            lock (unloadChunks)
            {
                if (unloadChunks.Count <= 0) { break; }
            }

            // grab a chunk to unload
            ChunkInfo chunkInfo;
            lock (unloadChunks)
            {
                chunkInfo = unloadChunks[0];
                chunkInfo.isLoaded = false;
                unloadChunks.RemoveAt(0);
            }

            lock (chunkInfo)
            {
                World.instance.RemoveChunk(chunkInfo.chunk);
                chunkInfo.chunk = null;

                lock (unloadChunks)
                {
                    chunkInfo.isUnloading = false;
                }
            }
        }

        int runs = 1; // load up to 5 chunks
        while (true)
        {
            lock (loadChunks)
            {
                if (loadChunks.Count <= 0) { break; }
            }

            // grab a chunk to load
            ChunkInfo chunkInfo;
            lock (loadChunks)
            {
                chunkInfo = loadChunks[0];
                loadChunks.RemoveAt(0);
            }

            lock (chunkInfo)
            {
                chunkInfo.chunk = World.instance.AddChunk(chunkInfo.data); //chunkInfo.data.Load(this);
                chunkInfo.data = null;

                lock (loadChunks)
                {
                    chunkInfo.isLoaded = true;
                    chunkInfo.isLoading = false;
                }
            }

            if ((runs--) <= 0)
            {
                break;
            }
        }


        //debug
        chunksStoreCount = chunksStore.Count;
        unloadChunksCount = unloadChunks.Count;
        loadChunksCount = loadChunks.Count;
    }
}

class ChunkInfo
{
    public ChunkData data;
    public Chunk chunk;
    public bool isLoading;
    public bool isLoaded;
    public bool isUnloading;
    public bool remove;

    public ChunkInfo(ChunkData data)
    {
        this.data = data;
        this.chunk = null;
        this.isLoading = false;
        this.isLoaded = false;
        this.isUnloading = false;
        this.remove = false;
    }
}
