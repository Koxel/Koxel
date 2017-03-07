using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {

    public Tile Tile;
    public Biome biome;
    public List<TileBehaviour> neighbours;
    public int moveCost;
    public Vector3 coordinates { get; set; }

    public virtual bool IsWalkable()
    {
        return true;
    }
    public virtual float MovementCost()
    {
        return 0;
    }
    public virtual List<TileBehaviour> Neighbours()
    {
        return neighbours;
    }
}

