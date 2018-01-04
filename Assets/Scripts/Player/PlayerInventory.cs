using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    public List<Item> inventory;

    private void Start()
    {
        inventory = new List<Item>();
    }

    public void Add(Item item, int slot = -1)
    {
        if (slot != -1)
        {
            inventory.Insert(slot, item);
        }
        else
        {
            inventory.Add(item);
        }
    }

    public void Remove(Item item)
    {
        inventory.Remove(item);
    }
}
