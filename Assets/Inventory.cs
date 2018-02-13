using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject uiPrefab;
    public InventoryUI inventoryUI;
    public int inventorySize = 20;
    public List<Item> items;

    private void Start()
    {
        items = new List<Item>();
    }

    public void Add(Item item, int slot = -1)
    {
        if (items.Count < inventorySize)
        {
            if (slot != -1)
            {
                items.Insert(slot, item);
            }
            else
            {
                int foundSpot = FindSpot();
                if (foundSpot == -1)
                {
                    items.Add(item);
                }
                else
                {
                    items[foundSpot] = item;
                }
            }
            UpdateUI();
        }
    }

    public void Remove(Item item)
    {
        int index = items.IndexOf(item);
        items[index] = null;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (inventoryUI != null)
            inventoryUI.UpdateUI();
    }

    public void ToggleUI()
    {
        if(inventoryUI == null)
        {
            //Open
            Game.instance.OpenUI();

            inventoryUI = Instantiate(uiPrefab, Game.instance.canvas.transform).GetComponent<InventoryUI>();
            inventoryUI.inventory = this;
            inventoryUI.UpdateUI();
        }
        else
        {
            //Close
            Game.instance.CloseUI();

            Destroy(inventoryUI.gameObject);
        }
    }

    int FindSpot()
    {
        foreach (Item item in items)
        {
            if(item == null)
            {
                return items.IndexOf(item);
            }
        }
        return -1;
    }
}
