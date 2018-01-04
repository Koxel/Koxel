using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AssetActions
{
    public class DropItems : IAssetAction
    {
        void IAssetAction.CallAction(Interactable target, Player source)
        {
            if (target.actionData == null)
                return;

            JToken jToken = target.actionData;
            if(jToken["DropItems"] != null)
            {
                ChanceItemConfig[] x = jToken["DropItems"].ToObject<ChanceItemConfig[]>();
                Game.instance.StartCoroutinePasser(Drop(x, target.transform));
            }
        }

        IEnumerator Drop(ChanceItemConfig[] items, Transform target)
        {
            Vector3 pos = target.GetChild(0).position;
            foreach (ChanceItemConfig item in items)
            {
                if (CheckChance(item.chance))
                {
                    GameObject GO;
                    //Debug.Log(item);
                    if (ModLoader.WorldItems.TryGetValue(item.item, out GO))
                    {
                        GameObject clone = GameObject.Instantiate(GO, pos, Quaternion.identity, World.instance.transform);
                        clone.transform.Rotate(new Vector3(0, Random.Range(0, 359), 0));
                        yield return new WaitForSeconds(.2f);
                    }
                }
            }
        }

        string IAssetAction.GetName()
        {
            return "DropItems";
        }

        bool CheckChance(float percentage)
        {
            float random = Random.Range(0f, 100f);
            if(random <= percentage)
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
