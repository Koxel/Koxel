using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour {

    public InventoryUI controller;
    public Image icon;
    Item item;

	public void Show(Item item)
    {
        if (item != null)
            Remove();
        this.item = item;
        icon.enabled = true;
        icon.sprite = item.icon.GetComponent<SpriteRenderer>().sprite;
    }

    public void Remove()
    {
        if (item != null)
        {
            this.item = null;
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    public void DestroyItem()
    {
        controller.inventory.Remove(item);
        controller.UpdateUI();
    }
}
