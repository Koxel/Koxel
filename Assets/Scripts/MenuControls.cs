using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControls : MonoBehaviour {

    public bool isOpenMenu;
    public GameObject escMenu;
    GameObject _openMenu;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!escMenu.activeSelf)
                OpenMenu(escMenu);
            else
                CloseMenu();
        }
    }

    void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
        _openMenu = menu;
        isOpenMenu = true;

        GetComponent<CameraController>().spectateCam.GetComponentInParent<Spectator>().spectating = false;
    }

    void CloseMenu()
    {
        _openMenu.SetActive(false);
        _openMenu = null;
        isOpenMenu = false;

        GetComponent<CameraController>().spectateCam.GetComponentInParent<Spectator>().spectating = GetComponent<CameraController>().spectateCam.enabled;
    }
}
