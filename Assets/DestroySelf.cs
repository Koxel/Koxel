using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetActions
{
    public class DestroySelf : IAssetAction
    {
        void IAssetAction.CallAction(Interactable target)
        {
            Debug.Log("BEEP DESTROYSELF");
            GameObject.Destroy(target.gameObject);
        }

        string IAssetAction.GetName()
        {
            return "DestroySelf";
        }
    }
}
