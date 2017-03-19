using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class Map : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject player;
    public Dictionary<Vector2, Tile> tileMap;
    public int radius;
    List<Tile> path;
    public float playerSpeed = .5f;
    public Tile currentTile;
    private int pathProgress = 0;
    public PlayerCam playerCam;
    public int maxMoveDist = 100;

    void Start () {
        path = new List<Tile>();
	}
	
	void Update () {
        // Move over the path
		if(path.Count > 0)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, path[pathProgress].transform.position, playerSpeed);

            if (Vector3.Distance(player.transform.position, path[pathProgress].transform.position) == 0)
                NextTile();
        }
	}

    // Pathfinding and such
    public void NextTile()
    {
        currentTile = path[pathProgress];
        if (pathProgress != path.Count-1)
            pathProgress = pathProgress + 1;
        else
            path = new List<Tile>();
    }

    public void PixelPath(Tile goal)
    {
        if (path.Count == 0)
        {
            path = CreateAStarPath(currentTile, goal);
            pathProgress = 0;
        }
    }

    public float HexDistance(Tile a, Tile b)
    {
        Vector3 cubeA = a.worldCoords;
        Vector3 cubeB = b.worldCoords;
        return Mathf.Max(Mathf.Abs(cubeA.x - cubeB.x), Mathf.Abs(cubeA.y - cubeB.y), Mathf.Abs(cubeA.z - cubeB.z));
    }

    // Fix by JohnyCilohokla (19-03-'17)
    public List<Tile> CreateAStarPath(Tile start, Tile goal)
    {
        FastPriorityQueue<Node> frontier;
        Dictionary<Tile, bool> visited = new Dictionary<Tile, bool>();
        List<Tile> path;
        Node current = new Node(start, 0);

        frontier = new FastPriorityQueue<Node>(maxMoveDist);
        frontier.Enqueue(current, 0);
        
        visited[start] = true;
        
        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            if (current.tile == goal)
                break;

            foreach (Tile next in GetNeighbours(current.tile))
            {
                var new_cost = current.cost + current.tile.moveCost;
                if (!visited.ContainsKey(next))
                {
                    visited[next] = true;

                    var heuristic = HexDistance(goal, next);
                    float priority = (float)new_cost + heuristic;
                    frontier.Enqueue(new Node(next, new_cost, current), priority);
                }
            }
        }

        if (current.tile!=goal) // path not found
        {
            return new List<Tile>(); // empty path
        }

        // current = goal;
        path = new List<Tile>();
        path.Add(current.tile);
        while(current.tile != start)
        {
            current = current.prev;
            path.Add(current.tile);
        }
        path.Add(start);
        path.Reverse();
        return path;
    }

    // TODO save neighbours to tile if there are 6, otherwise there are still some yet to be generated.
    List<Tile> GetNeighbours(Tile tile)
    {
        // Check if it has all possible neighbours...
        if (tile.neighbours.Count == 6)
            return tile.neighbours;

        // No, find as much as possible
        List<Tile> neighbours = new List<Tile>();
        //  0, -1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x, tile.worldCoords.y - 1)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x, tile.worldCoords.y - 1)]);
        // +1, -1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y - 1)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y - 1)]);
        // -1,  0
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y)]);
        // +1,  0
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y)]);
        // -1, +1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y + 1)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y + 1)]);
        //  0, +1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x, tile.worldCoords.y + 1)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x, tile.worldCoords.y + 1)]);

        // If we found all neighbours now, save them to the tile
        if (neighbours.Count == 6)
            tile.neighbours = neighbours;

        return neighbours;
    }
}