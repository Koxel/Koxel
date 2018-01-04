using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string description;
    public GameObject icon;
    public GameObject model;

    public Item(string name, string description, GameObject icon, GameObject model)
    {
        this.name = name;
        this.description = description;
        this.icon = icon;
        this.model = model;
    }
}
