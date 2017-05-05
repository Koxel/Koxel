﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class Map : MonoBehaviour {
    public GameObject player;
    public Dictionary<Vector2, Tile> tileMap;
    public Dictionary<Vector2, Chunk> chunkMap;
    public int radius;
    List<Tile> path;
    public float playerSpeed = .5f;
    public Tile currentTile;
    int pathProgress = 0;
    public int maxMoveDist = 100;
    public bool moving = false;

    void Start ()
    {
        path = new List<Tile>();
        tileMap = new Dictionary<Vector2, Tile>();
        chunkMap = new Dictionary<Vector2, Chunk>();
    }
	
	void Update ()
    {
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
        if (pathProgress != path.Count - 1)
            pathProgress = pathProgress + 1;
        else // We're there
        {
            path = new List<Tile>();
            moving = false;
        }
    }

    public void PathTo(Tile goal)
    {
        if (path.Count == 0 && HexDistance(goal, currentTile) < 50)
        {
            moving = true;
            path = AStar(currentTile, goal);
            pathProgress = 0;
        }
    }

    public float HexDistance(Tile tileA, Tile tileB)
    {
        Vector3 a = tileA.worldCoords;
        Vector3 b = tileB.worldCoords;
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
    }

    // Fix by JohnyCilohokla (19-03-'17)
    public List<Tile> AStar(Tile start, Tile goal)
    {
        if (HexDistance(start, goal) <= maxMoveDist)
        {
            //Debug.Log(HexDistance(start, goal));
            FastPriorityQueue<Node> frontier;
            Dictionary<Tile, bool> visited = new Dictionary<Tile, bool>();
            List<Tile> path;
            Node current = new Node(start, 0);

            frontier = new FastPriorityQueue<Node>(maxMoveDist*6);
            frontier.Enqueue(current, 0);

            visited[start] = true;

            while (frontier.Count > 0)
            {
                current = frontier.Dequeue();

                if (current.tile == goal)
                    break;

                foreach (Tile next in GetNeighbours(current.tile))
                {
                    if (next == null)
                    {
                        Debug.Log(next + " from " + current.tile);
                        current.tile.SetColor(new Color());
                    }
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

            if (current.tile != goal) // path not found
            {
                return new List<Tile>(); // empty path
            }

            // current = goal;
            path = new List<Tile>();
            path.Add(current.tile);
            while (current.tile != start)
            {
                current = current.prev;
                path.Add(current.tile);
            }
            path.Add(start);
            path.Reverse();
            return path;
        }
        return new List<Tile>();
    }

    // TODO save neighbours to tile if there are 6, otherwise there are still some yet to be generated.
    public List<Tile> GetNeighbours(Tile tile)
    {
        // Check if it has all possible neighbours...
        if (tile.neighbours.Count == 6)
            return tile.neighbours;

        // No, find as much as possible
        List<Tile> neighbours = new List<Tile>();
        //  0, -1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x, tile.worldCoords.y - 1)))
        {
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x, tile.worldCoords.y - 1)]);
            if (tile.worldCoords == new Vector3(8,0,-8))
                Debug.Log((tile.worldCoords.x) + ", " + (tile.worldCoords.y - 1) + " is null");
        }
        // +1, -1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y - 1)))
        {
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y - 1)]);
            if (tile.worldCoords == new Vector3(8, 0, -8))
                Debug.Log((tile.worldCoords.x+1) + ", " + (tile.worldCoords.y - 1) + " is null");
        }
        // -1,  0
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y)))
        {
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y)]);
            if (tile.worldCoords == new Vector3(8, 0, -8))
                Debug.Log((tile.worldCoords.x-1) + ", " + (tile.worldCoords.y) + " is null");
        }
        // +1,  0
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y)))
        {
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y)]);
            if (tile.worldCoords == new Vector3(8, 0, -8))
                Debug.Log((tile.worldCoords.x+1) + ", " + (tile.worldCoords.y) + " is null");
        }
        // -1, +1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y + 1)))
        {
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y + 1)]);
            if (tile.worldCoords == new Vector3(8, 0, -8))
                Debug.Log((tile.worldCoords.x-1) + ", " + (tile.worldCoords.y+1) + " is null");
        }
        //  0, +1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x, tile.worldCoords.y + 1)))
        {
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x, tile.worldCoords.y + 1)]);
            if (tile.worldCoords == new Vector3(8, 0, -8))
                Debug.Log((tile.worldCoords.x) + ", " + (tile.worldCoords.y + 1) + " is null");
        }
        /*// +2, -1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x + 2, tile.worldCoords.y - 1)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x + 2, tile.worldCoords.y - 1)]);
        //  +1, -2
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y - 2)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y - 2)]);
        //  -1, -1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y - 1)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y - 1)]);
        //  -2, +1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x - 2, tile.worldCoords.y + 1)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x - 2, tile.worldCoords.y + 1)]);
        //  -1, -2
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y + 2)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x - 1, tile.worldCoords.y + 2)]);
        //  +1, +1
        if (tileMap.ContainsKey(new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y + 1)))
            neighbours.Add(tileMap[new Vector2(tile.worldCoords.x + 1, tile.worldCoords.y + 1)]);*/

        // If we found all neighbours now, save them to the tile
        if (neighbours.Count == 6) ;
            //tile.neighbours = neighbours;

        return neighbours;
    }

    public void UpdateChunkNeighbours(Chunk chunk)
    {
        if (chunk == null)
            return;
        if(chunk.neighbours.Count != 6)
        {
            Chunk foundChunk;
            //  0, -1
            if (chunkMap.ContainsKey(new Vector2(chunk.coords.x, chunk.coords.y - 1)))
            {
                foundChunk = chunkMap[new Vector2(chunk.coords.x, chunk.coords.y - 1)];
                if (!chunk.neighbours.Contains(foundChunk))
                    chunk.neighbours.Add(foundChunk);
                if (!foundChunk.neighbours.Contains(chunk))
                    foundChunk.neighbours.Add(chunk);
            }
            // +1, -1
            if (chunkMap.ContainsKey(new Vector2(chunk.coords.x + 1, chunk.coords.y - 1)))
            {
                foundChunk = chunkMap[new Vector2(chunk.coords.x + 1, chunk.coords.y - 1)];
                if (!chunk.neighbours.Contains(foundChunk))
                    chunk.neighbours.Add(foundChunk);
                if (!foundChunk.neighbours.Contains(chunk))
                    foundChunk.neighbours.Add(chunk);
            }
            // -1,  0
            if (chunkMap.ContainsKey(new Vector2(chunk.coords.x - 1, chunk.coords.y)))
            {
                foundChunk = chunkMap[new Vector2(chunk.coords.x - 1, chunk.coords.y)];
                if (!chunk.neighbours.Contains(foundChunk))
                    chunk.neighbours.Add(foundChunk);
                if (!foundChunk.neighbours.Contains(chunk))
                    foundChunk.neighbours.Add(chunk);
            }
            // +1,  0
            if (chunkMap.ContainsKey(new Vector2(chunk.coords.x + 1, chunk.coords.y)))
            {
                foundChunk = chunkMap[new Vector2(chunk.coords.x + 1, chunk.coords.y)];
                if (!chunk.neighbours.Contains(foundChunk))
                    chunk.neighbours.Add(foundChunk);
                if (!foundChunk.neighbours.Contains(chunk))
                    foundChunk.neighbours.Add(chunk);
            }
            // -1, +1
            if (chunkMap.ContainsKey(new Vector2(chunk.coords.x - 1, chunk.coords.y + 1)))
            {
                foundChunk = chunkMap[new Vector2(chunk.coords.x - 1, chunk.coords.y + 1)];
                if (!chunk.neighbours.Contains(foundChunk))
                    chunk.neighbours.Add(foundChunk);
                if (!foundChunk.neighbours.Contains(chunk))
                    foundChunk.neighbours.Add(chunk);
            }
            //  0, +1
            if (chunkMap.ContainsKey(new Vector2(chunk.coords.x, chunk.coords.y + 1)))
            {
                foundChunk = chunkMap[new Vector2(chunk.coords.x, chunk.coords.y + 1)];
                if (!chunk.neighbours.Contains(foundChunk))
                    chunk.neighbours.Add(foundChunk);
                if (!foundChunk.neighbours.Contains(chunk))
                    foundChunk.neighbours.Add(chunk);
            }

        }
    }
}