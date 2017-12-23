using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AssetInteraction
{
    public string name;
    public GameObject sprite;

    public AssetInteraction(string name, GameObject sprite)
    {
        this.name = name;
        this.sprite = sprite;
    }
}