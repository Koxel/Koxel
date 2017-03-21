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
        hoverList = new List<Tile>();
        prevHoverList = new List<Tile>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!map.moving)
            DrawMousePath();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            //Debug.DrawRay(ray.origin, ray.direction * 5000, Color.cyan, 5f);

            if (Physics.Raycast(ray, out hitInfo))
            {
                map.PathTo(hitInfo.collider.transform.parent.GetComponent<Tile>());
            }
        }
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
                    map.PathTo(hitInfo.collider.transform.parent.GetComponent<Tile>());
                }
            }
        }
    }

    List<Tile> hoverList;
    List<Tile> prevHoverList;
    Tile prevHitTile;

    void DrawMousePath()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Tile hitTile = hitInfo.collider.transform.parent.GetComponent<Tile>();
            hoverList = map.AStar(map.currentTile, hitTile);
            if (hitTile != prevHitTile)
            {
                foreach (Tile tile in prevHoverList)
                {
                    tile.SetColor(tile.tileType.defaultColor);
                }

                foreach (Tile tile in hoverList)
                {
                    tile.SetColor(tile.tileType.hoverColor);
                }
                prevHoverList = hoverList;
                prevHitTile = hitTile;
            }
            
        }
        else
        {
            if (hoverList.Count > 0)
            {
                foreach (Tile tile in hoverList)
                {
                    tile.SetColor(tile.tileType.defaultColor);
                    
                }
                hoverList.Clear();
            }
        }
    }
}
