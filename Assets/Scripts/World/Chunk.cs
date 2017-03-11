using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour{
    public Biome biome;
    public List<Chunk> neighbours;
    public List<TileBehaviour> tiles;
    public Vector2 coords;

    /*public Chunk(Biome biome, int radius)
    {

    }*/
}
