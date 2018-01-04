using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    public Inventory inventory;

	public void UpdateUI()
    {
        for (int i = 0; i < inventory.inventorySize; i++)
        {
            InventoryUISlot slot = transform.GetChild(0).GetChild(i).GetComponent<InventoryUISlot>();
            if(i < inventory.items.Count)
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
        foreach(Item item in inventory.items)
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
        }
    }
}
