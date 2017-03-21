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
        float random = Random.Range(-0.05f, 0.05f);
        Color newColor = new Color(color.r + random, color.g + random, color.b + random, color.a);
        transform.GetChild(0).GetComponent<Renderer>().materials[1].color = newColor;
    }

    /*void Update()
    {
        WorldGenerator map = GameObject.Find("World").GetComponent<WorldGenerator>();
        transform.position = new Vector3(transform.position.x, map.noise((int)worldCoords.x, (int)worldCoords.y), transform.position.z);
    }*/
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

