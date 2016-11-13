using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileType {

    public string name;
    public GameObject tilePrefab;

    public bool isWalkable;
    public float movementCost = 1;

}
