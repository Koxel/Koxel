using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetActions
{
    public class AddToInv : IAssetAction
    {
        void IAssetAction.CallAction(Interactable target)
        {

        }

        string IAssetAction.GetName()
        {
            return "AddToInv";
        }
    }
}