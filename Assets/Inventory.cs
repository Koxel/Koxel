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
                items.Add(item);
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
            Debug.Log("Spawn");
            inventoryUI = Instantiate(uiPrefab, Game.instance.canvas.transform).GetComponent<InventoryUI>();
            inventoryUI.inventory = this;
            inventoryUI.UpdateUI();
        }
        else
        {
            Destroy(inventoryUI.gameObject);
        }
    }
}
