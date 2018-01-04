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
    }

    public void Activate(Interactable interactable)
    {
        foreach(IAssetAction action in actionSequence)
        {
            action.CallAction(interactable);
        }
    }
}