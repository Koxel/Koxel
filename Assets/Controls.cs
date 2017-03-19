using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
    public Map map;
    GameObject canvas;
    public string EscapeMenu;
    bool open_EscapeMenu = false;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject menu = canvas.transform.FindChild(EscapeMenu).gameObject;
            menu.SetActive(!open_EscapeMenu);
            open_EscapeMenu = !open_EscapeMenu;
        }
        if (!open_EscapeMenu)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    map.PixelPath(hitInfo.collider.transform.parent.GetComponent<Tile>());
                }
            }
        }
    }
}
