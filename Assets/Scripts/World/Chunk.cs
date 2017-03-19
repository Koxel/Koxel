using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour{
    public Biome biome;
    public List<Chunk> neighbours;
    public List<Tile> tiles;
    public Vector2 coords;
}
