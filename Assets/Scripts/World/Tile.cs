using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class Tile : MonoBehaviour
{
    public TileType tileType;
    public Chunk chunk;
    public List<Tile> neighbours;
    public float moveCost;
    public Vector3 chunkCoords;
    public Vector3 worldCoords;
    public bool walkable;
    
    public void SetColor(Color color)
    {
        transform.GetChild(0).GetComponent<Renderer>().materials[1].color = color;
    }
}

public class Node : FastPriorityQueueNode
{
    public Tile tile;
    public Node prev;
    public float cost;

    public Node(Tile tile, float cost, Node prev = null)
    {
        this.tile = tile;
        this.prev = prev;
        this.cost = cost;
    }
}

