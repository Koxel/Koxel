using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour{
    public List<Chunk> neighbours;
    public Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
    public Vector2 coords;
}
