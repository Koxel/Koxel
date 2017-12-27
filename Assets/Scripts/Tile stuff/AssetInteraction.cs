using AssetActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetInteraction
{
    public string name;
    public GameObject sprite;

    public List<IAssetAction> actionSequence;

    public AssetInteraction(string name, GameObject sprite, List<IAssetAction> actionSequence)
    {
        this.name = name;
        this.sprite = sprite;
        this.actionSequence = new List<IAssetAction>(actionSequence);
        //this.actionSequence.AddRange(actionSequence);
        if(name == "Smash")
        {
            foreach(IAssetAction ac in actionSequence)
            {
                Debug.Log(ac);
            }
        }
    }

    public void Activate(Interactable interactable)
    {
        Debug.Log(name);
        Debug.Log(this.actionSequence);
        foreach(IAssetAction action in this.actionSequence)
        {
            action.CallAction(interactable);
            Debug.Log(action.GetName());
        }
    }
}