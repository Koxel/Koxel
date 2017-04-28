using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour{
    public List<Chunk> neighbours;
    public List<Tile> tiles = new List<Tile>();
    public Vector2 coords;
}
