using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileAsset : MonoBehaviour {

    public string name;
    public GameObject prefab;
    public int chance;
    public Vector2 sizeRange;

    public TileAsset(string name, GameObject prefab, int chance, Vector2 sizeRange)
    {
        this.name = name;
        this.prefab = prefab;
        this.chance = chance;
        this.sizeRange = sizeRange;
    }

    /*public GameObject Spawn(Tile tile, Vector3 position, Vector3 eulerangles)
    {
        GameObject GO = Instantiate()
    }*/
}
