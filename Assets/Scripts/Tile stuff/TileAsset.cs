using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class TileAsset : Interactable
{
    public GameObject prefab;
    public int chance;
    public Vector2 sizeRange;

    public void Setup(string name, GameObject prefab, int chance, Vector2 sizeRange, List<AssetInteraction> assetInteractions, JToken actionData)
    {
        this.name = name;
        this.prefab = prefab;
        this.chance = chance;
        this.sizeRange = sizeRange;
        this.assetInteractions = new List<AssetInteraction>();
        this.assetInteractions.AddRange(assetInteractions);
        this.actionData = actionData;
    }
}
