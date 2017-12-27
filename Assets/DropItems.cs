using UnityEngine;
using Newtonsoft.Json.Linq;

namespace AssetActions
{
    [System.Serializable]
    public class DropItems : IAssetAction
    {
        void IAssetAction.CallAction(Interactable target)
        {
            Debug.Log("BEEP DROPITEMS");
        }

        string IAssetAction.GetName()
        {
            return "DropItems";
        }
    }
}
