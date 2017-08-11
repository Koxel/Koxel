using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileAsset {

    public GameObject prefab;
    [Header("Higher nr = lower chance..")]
    public int chance = 6;
    public Vector2 sizeRanges;

}
