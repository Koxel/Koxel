using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAsset : MonoBehaviour {

    public string name;
    public GameObject prefab;
    

    public TileAsset(string name, GameObject prefab)
    {
        this.name = name;
        this.prefab = prefab;
    }

}
