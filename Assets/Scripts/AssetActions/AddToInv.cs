using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetActions
{
    public class AddToInv : IAssetAction
    {
        void IAssetAction.CallAction(Interactable target, Player source)
        {
            if (target.actionData == null)
                return;

            JToken jToken = target.actionData;
            if (jToken["AddToInv"] != null)
            {
                ChanceItemConfig[] items = jToken["AddToInv"].ToObject<ChanceItemConfig[]>();
                foreach (ChanceItemConfig item in items)
                {
                    if (CheckChance(item.chance))
                    {
                        Item item_;
                        if (ModLoader.Items.TryGetValue(item.item, out item_))
                        {
                            source.inventory.Add(item_);
                        }
                    }
                }
            }
        }

        string IAssetAction.GetName()
        {
            return "AddToInv";
        }

        bool CheckChance(float percentage)
        {
            float random = Random.Range(0f, 100f);
            if (random <= percentage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}