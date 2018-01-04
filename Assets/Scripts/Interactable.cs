using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public new string name;
    [System.NonSerialized]
    public List<AssetInteraction> assetInteractions;
    public JToken actionData;

}
