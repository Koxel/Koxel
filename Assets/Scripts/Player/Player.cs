using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerUIController UIcontroller;

    public PlayerController controller;
    public Inventory inventory;
    public PlayerCrafting crafting;

    private bool uiOpen;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Press I");
            if (uiOpen)
            {
                uiOpen = false;
                UIcontroller.CloseUI();
            }
            else
            {
                uiOpen = true;
                UIcontroller.OpenUI(PlayerUIController.UI.Inventory);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape) && uiOpen)
        {
            uiOpen = false;
            UIcontroller.CloseUI();
        }
    }
}
