using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour {

    public TileType[] tileTypes;

    public GameObject playerPrefab;
    public GameObject player;

    int[,] tiles;
    public Node[,] graph;

    public int mapWidth = 50;
    public int mapHeight = 50;

    public float hexOffsetX;
    public float hexOffsetY;
    public float hexOddRowOffsetX;
     
    void Start()
    {
        // Setup the map
        SetupMap();
        
        // Spawn prefabs
        GenerateMap();

        // Setup the map for path finding
        GeneratePathFindingGraph();

        // Spawn Player
        SetupPlayer();
    }

    void SetupPlayer()
    {
        player = (GameObject)Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity);
        player.transform.parent = this.transform;

        // Give the player its coords
        player.GetComponent<Unit>().tileX = 0;
        player.GetComponent<Unit>().tileY = 0;

        // Give the player a reference to the map
        player.GetComponent<Unit>().map = this;
    }

    void SetupMap()
    {
        // Allocate map tiles
        tiles = new int[mapWidth, mapHeight];

        // Everything Grass
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                tiles[x, y] = 0;
            }
        }

        // Some Lake
        tiles[4, 4] = 1;
        tiles[5, 4] = 1;
        tiles[6, 4] = 1;
        tiles[7, 4] = 1;
        tiles[8, 4] = 1;

        tiles[4, 5] = 1;
        tiles[4, 6] = 1;
        tiles[8, 5] = 1;
        tiles[4, 6] = 1;
    }

    void GenerateMap() {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];
                Vector3 hexPos = new Vector3(0,0,0);

                if (y % 2 == 0 || y == 0) //Even
                    hexPos = new Vector3(x * hexOffsetX, 0, y * hexOffsetY);
                else if (y % 2 == 1) //Odd
                    hexPos = new Vector3(x * hexOffsetX + hexOddRowOffsetX, 0, y * hexOffsetY);

                GameObject hex = (GameObject)Instantiate(tt.tilePrefab, hexPos, Quaternion.identity);

                hex.name = tt.name + " (" + x + ", " + y + ")";
                hex.transform.parent = transform;

                hex.GetComponent<Hex>().type = tiles[x, y];
                hex.GetComponent<Hex>().x = x;
                hex.GetComponent<Hex>().y = y;
            }
        }
    }

    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {

        TileType tt = tileTypes[tiles[targetX, targetY]];

        if (UnitCanEnterTile(targetX, targetY) == false)
            return Mathf.Infinity;

        float cost = tt.movementCost;

        if (sourceX != targetX && sourceY != targetY)
        {
            // We are moving diagonally!  Fudge the cost for tie-breaking
            // Purely a cosmetic thing!
            cost += 0.001f;
        }

        return cost;

    }

    public bool UnitCanEnterTile(int x, int y)
    {

        // We could test the unit's walk/hover/fly type against various
        // terrain flags here to see if they are allowed to enter the tile.

        return tileTypes[tiles[x, y]].isWalkable;
    }

    void GeneratePathFindingGraph()
    {
        graph = new Node[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // 6 way connections
                // Left
                if (x > 0)
                {
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                }
                // Right
                if (x < mapWidth - 1)
                {
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                    if (y > 0)
                        graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
                    if (y < mapHeight - 1)
                        graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
                }
                // Down
                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                // Up
                if (y < mapHeight - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);
            }
        }

    }

    public void CalculatePath(int x, int y)
    {
       player.GetComponent<Unit>().currentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        Node source = graph[
                                player.GetComponent<Unit>().tileX,
                                player.GetComponent<Unit>().tileY
                            ];

        Node target = graph[x, y];

        dist[source] = 0;
        prev[source] = null;

        foreach(Node v in graph)
        {
            if(v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while(unvisited.Count > 0)
        {
            // unvisted node with smallest dist
            Node u = null;

            foreach(Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            // We're there
            if(u == target)
            {
                break;
            }

            unvisited.Remove(u);

            foreach(Node v in u.neighbours)
            {
                if (tileTypes[tiles[v.x, v.y]].isWalkable)
                {
                    float alt = dist[u] + u.DistanceTo(v);

                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }
        }

        // Shortest or no path
        if(prev[target] == null)
        {
            // No path
            return;
        }

        // yay there is one
        player.GetComponent<Unit>().currentPath = new List<Node>();

        Node curr = target;

        while(curr != null)
        {
            player.GetComponent<Unit>().currentPath.Add(curr);
            curr = prev[curr];
        }
        // now its target -> source so reverse
        player.GetComponent<Unit>().currentPath.Reverse();
    }

    public void MoveNextTilePlayer()
    {
        player.GetComponent<Unit>().MoveNextTile();
    }
}
