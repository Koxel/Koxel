using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour {

    public PlayerCrafting playerCrafting;
    public int rowAmount = 7;

    public void UpdateUI()
    {
        /*int rows = (int)(playerCrafting.recipes.Count / 7f); 
        for (int i = 0; i < rows; i++)
        {
            CraftingUISlot slot = transform.GetChild(0).GetChild(i).GetComponent<CraftingUISlot>();
            if (i < playerCrafting.recipes.Count)
            {
                Item item = inventory.items[i];
                if (item != null)
                {
                    slot.Show(item);
                }
                else
                {
                    slot.Remove();
                }
            }
            else
            {
                slot.Remove();
            }
        }
        foreach (Item item in inventory.items)
        {
            InventoryUISlot slot = transform.GetChild(0).GetChild(inventory.items.IndexOf(item)).GetComponent<InventoryUISlot>();
            if (item != null)
            {
                slot.Show(item);
            }
            else
            {
                slot.Remove();
            }
        }*/
    }
}
