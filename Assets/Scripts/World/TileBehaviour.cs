using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {

    public Tile Tile;
    public Chunk chunk;
    public List<TileBehaviour> neighbours;
    public int moveCost;
    public Vector3 chunkCoords { get; set; }
    public Vector3 worldCoords { get; set; }
    public bool walkable;
}

