using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class Tile : MonoBehaviour
{
    public TileType tileType;
    public Chunk chunk;
    public List<Tile> neighbours;
    public int moveCost;
    public Vector3 chunkCoords;
    public Vector3 worldCoords;
    public bool walkable;
    // public Node node;
}

public class Node : FastPriorityQueueNode
{
    public Tile tile;
    public Node prev;
    public int cost;

    public Node(Tile tile, int cost, Node prev = null)
    {
        this.tile = tile;
        this.prev = prev;
        this.cost = cost;
    }
}

