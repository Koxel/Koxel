using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUIController : MonoBehaviour {
    [Header("References")]
    public GameObject Frame;
    public GameObject Inventory;
    public GameObject Outfit;
    public GameObject Crafting;
    public GameObject Journal;
    public GameObject Controls;
    public GameObject Settings;
    public GameObject Menu;

    [HideInInspector]
    public Player player;

    [HideInInspector]
    public UI openedUI;

    public enum UI
    {
        None,
        Inventory, Outfit, Crafting, Journal,
        Controls, Settings, Menu
    }

    private void Start()
    {
        CloseUI();
    }
    
    public void OpenUI(UI ui)
    {
        Game.instance.OpenUI();
        Frame.SetActive(true);
        Debug.Log("Open");
        switch (ui)
        {
            case UI.Inventory:
                openedUI = UI.Inventory;
                Inventory.SetActive(true);
                Debug.Log("Open Inv");
                InventoryUI UI_ = Inventory.GetComponent<InventoryUI>();
                UI_.inventory = player.inventory;
                UI_.SetupSlots();
                UI_.UpdateUI();
                break;
            case UI.Outfit:
                openedUI = UI.Outfit;
                Outfit.SetActive(true);

                break;
            case UI.Crafting:
                openedUI = UI.Crafting;
                Crafting.SetActive(true);

                break;
            case UI.Journal:
                openedUI = UI.Journal;
                Journal.SetActive(true);

                break;
            case UI.Controls:
                openedUI = UI.Controls;
                Controls.SetActive(true);

                break;
            case UI.Settings:
                openedUI = UI.Settings;
                Settings.SetActive(true);

                break;
            case UI.Menu:
                openedUI = UI.Menu;
                Menu.SetActive(true);

                break;
            default:
                openedUI = UI.None;
                CloseUI();
                break;

        }
    }

    public void ChangeTabByButton()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        switch (button.transform.GetChild(0).name)
        {
            case "Inventory":
                ChangeTab(UI.Inventory);
                break;
            case "Crafting":
                ChangeTab(UI.Crafting);
                break;
            case "Settings":
                ChangeTab(UI.Settings);
                break;
            default:
                CloseUI();
                break;
        }
    }

    public void ChangeTab(UI ui)
    {
        //Close prev
        if(Inventory != null)
            Inventory.SetActive(false);
        if(Outfit != null)
            Outfit.SetActive(false);
        if(Crafting != null)
            Crafting.SetActive(false);
        if(Journal != null)
            Journal.SetActive(false);
        if(Controls != null)
            Controls.SetActive(false);
        if(Settings != null)
            Settings.SetActive(false);
        if(Menu != null)
            Menu.SetActive(false);

        OpenUI(ui);
    }

    public void CloseUI()
    {
        if (Inventory != null)
            Inventory.SetActive(false);
        if (Outfit != null)
            Outfit.SetActive(false);
        if (Crafting != null)
            Crafting.SetActive(false);
        if (Journal != null)
            Journal.SetActive(false);
        if (Controls != null)
            Controls.SetActive(false);
        if (Settings != null)
            Settings.SetActive(false);
        if (Menu != null)
            Menu.SetActive(false);

        Frame.SetActive(false);
        openedUI = UI.None;
        Game.instance.CloseUI();
    }
}
