using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetInteraction : MonoBehaviour
{
    public string name;
    public GameObject model;

    public void Setup(Asset_Interaction data)
    {
        Reset();

        this.name = data.name;
        this.model = data.model;

        //Create model
        Instantiate(model, transform);

    }

    public void Reset()
    {
        this.name = null;
        this.model = null;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}

[System.Serializable]
public class Asset_Interaction
{
    public string name;
    public GameObject model;

    public Asset_Interaction(string name, GameObject model)
    {
        this.name = name;
        this.model = model;
    }
}