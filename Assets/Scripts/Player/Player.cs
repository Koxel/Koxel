using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerController controller;
    public Inventory inventory;

    private void Awake()
    {
        //Hook up to events
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.ToggleUI();
        }
    }
}
