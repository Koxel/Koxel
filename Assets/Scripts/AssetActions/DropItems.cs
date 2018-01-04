using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AssetActions
{
    public class DropItems : IAssetAction
    {
        void IAssetAction.CallAction(Interactable target)
        {
            JToken jToken = target.actionData;
            if(jToken["DropItems"] != null)
            {
                DropItemConfig[] x = jToken["DropItems"].ToObject<DropItemConfig[]>();
                Game.instance.StartCoroutinePasser(Drop(x, target.transform));
            }
        }

        IEnumerator Drop(DropItemConfig[] items, Transform target)
        {
            Vector3 pos = target.GetChild(0).position;
            foreach (DropItemConfig item in items)
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

    class DropItemConfig
    {
        public string item;
        public int chance;
    }
}
