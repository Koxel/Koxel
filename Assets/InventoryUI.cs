using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : PlayerUI {
    
    public Inventory inventory;

	public override void UpdateUI()
    {
        //do we have enough rows?
        if(inventory.inventorySize / rowWidth > createdRows)
        {
            //how many extra do we need?
            int extraRows = inventory.inventorySize / rowWidth - createdRows;
            
            for (int i = 0; i < extraRows; i++)
                AddRow();
        }

        for (int i = 0; i < inventory.inventorySize; i++)
        {
            InventoryUISlot slot = SlotsParent.GetChild(i).GetComponent<InventoryUISlot>();
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
        
        //this part breaks
        foreach(Item item in inventory.items)
        {
            InventoryUISlot slot = SlotsParent.GetChild(inventory.items.IndexOf(item)).GetComponent<InventoryUISlot>();
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
