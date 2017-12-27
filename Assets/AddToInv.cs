using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetActions
{
    [System.Serializable]
    public class AddToInv : IAssetAction
    {
        void IAssetAction.CallAction(Interactable target)
        {
            Debug.Log("BEEP ADDTOINV");
        }

        string IAssetAction.GetName()
        {
            return "AddToInv";
        }
    }
}