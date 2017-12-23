using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAsset : Interactable
{
    public GameObject prefab;
    public int chance;
    public Vector2 sizeRange;

    public void Setup(string name, GameObject prefab, int chance, Vector2 sizeRange, List<AssetInteraction> assetInteractions)
    {
        this.name = name;
        this.prefab = prefab;
        this.chance = chance;
        this.sizeRange = sizeRange;
        this.assetInteractions = new List<AssetInteraction>();
        this.assetInteractions.AddRange(assetInteractions);
    }
}
