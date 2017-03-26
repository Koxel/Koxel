using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
    public Map map;
    GameObject canvas;
    public string EscapeMenu;
    bool menuOpen = false;
    public GameObject player;
    bool followPlayer = true;
    Vector3 playerOffset;

    // Use this for initialization
    void Start () {
        canvas = GameObject.Find("Canvas");
        hoverList = new List<Tile>();
        prevHoverList = new List<Tile>();
        player = GameObject.Find("Player");
        playerOffset = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (followPlayer)
        {
            FollowPlayer();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject menu = canvas.transform.FindChild(EscapeMenu).gameObject;
            menu.SetActive(!menuOpen);
            menuOpen = !menuOpen;
        }
        if (!menuOpen)
        {
            if (!map.moving)
            {
                DrawMousePath();

                if (Input.GetMouseButtonDown(0))
                {
                    LClick();
                }

                

                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    CameraMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                }
            }
        }
    }

    void CameraMovement(float hor, float ver)
    {
        followPlayer = false;

        float interpolation = 10f * Time.deltaTime;
        Vector3 pos = transform.parent.position;
        pos.x = Mathf.Lerp(pos.x, pos.x + hor*2, interpolation);
        pos.z = Mathf.Lerp(pos.z, pos.z + ver*2, interpolation);
        transform.parent.position = pos;
    }

    void LClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            followPlayer = true;
            map.PathTo(hitInfo.collider.transform.parent.GetComponent<Tile>());
        }
    }

    public float followSpeed = 2;
    void FollowPlayer()
    {
        float interpolation = followSpeed * Time.deltaTime;

        Vector3 position = this.transform.position;
        position.x = Mathf.Lerp(transform.position.x, player.transform.position.x + playerOffset.x, interpolation);
        //position.y = Mathf.Lerp(transform.position.y, player.transform.position.y + playerOffset.y, interpolation);
        position.z = Mathf.Lerp(transform.position.z, player.transform.position.z + playerOffset.z, interpolation);

        transform.position = position;
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
