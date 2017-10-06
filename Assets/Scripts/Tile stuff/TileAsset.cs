using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAsset : MonoBehaviour
{
    public string name;
    public GameObject prefab;
    public int chance;
    public Vector2 sizeRange;
    public List<Asset_Interaction> assetInteractions;// = new List<Asset_Interaction>();

    public void Setup(string name, GameObject prefab, int chance, Vector2 sizeRange, List<Asset_Interaction> assetInteractions)
    {
        this.name = name;
        this.prefab = prefab;
        this.chance = chance;
        this.sizeRange = sizeRange;
        this.assetInteractions = new List<Asset_Interaction>();
        this.assetInteractions.AddRange(assetInteractions);
    }
}
