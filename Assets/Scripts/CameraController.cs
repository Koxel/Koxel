using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Game Game;
    Map Map;
    LoadUnload loader;
    public Camera spectateCam;
    public Camera playerCam;
    Chunk currentChunk;

    void Start()
    {
        Game = GetComponent<Game>();
        loader = GetComponent<LoadUnload>();
        Map = transform.FindChild("World").GetComponent<Map>();
        currentChunk = null;
    }

    public void Setup (GameObject player)
    {
        spectateCam = transform.FindChild("Spectator Camera").transform.GetChild(0).GetComponent<Camera>();
        playerCam = transform.FindChild("Player Camera").transform.GetChild(0).GetComponent<Camera>();

        spectateCam.enabled = false;
        spectateCam.GetComponent<AudioListener>().enabled = false;
        spectateCam.GetComponentInParent<Spectator>().spectating = false;

        playerCam.enabled = true;
        playerCam.GetComponent<AudioListener>().enabled = true;
        GetComponent<LoadUnload>().loader = playerCam.transform.parent.gameObject;

        playerCam.transform.parent.GetComponent<Follower>().followThis = player;

        currentChunk = Map.currentTile.chunk;
    }
	
	void Update ()
    {
        if (GetComponent<MenuControls>().isOpenMenu)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            foreach (Tile tile in prevHoverList)
                tile.SetColor(tile.tileType.defaultColor);

            spectateCam.transform.parent.position = playerCam.transform.position;

            //Switch Camera
            spectateCam.enabled = !spectateCam.enabled;
            spectateCam.GetComponent<AudioListener>().enabled = spectateCam.enabled;
            spectateCam.GetComponentInParent<Spectator>().spectating = spectateCam.enabled;

            playerCam.enabled = !playerCam.enabled;
            playerCam.GetComponent<AudioListener>().enabled = playerCam.enabled;
            
            if(spectateCam.enabled)
                GetComponent<LoadUnload>().loader = spectateCam.transform.parent.gameObject;
            else
                GetComponent<LoadUnload>().loader = playerCam.transform.parent.GetComponent<Follower>().followThis;
        }
        if (playerCam.enabled)
        {
            if(!Game.World.GetComponent<Map>().moving)
                DrawMousePath();

            if (Input.GetMouseButtonDown(0))
                CreatePath();
        }

        if (spectateCam.enabled)
        {
            RaycastHit hit;
            if (Physics.Raycast(spectateCam.transform.position, spectateCam.transform.forward, out hit, Mathf.Infinity, 1 << 8))
            {
                Tile tile = hit.transform.parent.GetComponent<Tile>();
                if (tile.chunk != currentChunk)
                {
                    currentChunk = tile.chunk;
                    loader.ChunkChanged(currentChunk);
                }
            }
        }
	}

    void CreatePath()
    {
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
            Game.World.GetComponent<Map>().PathTo(hitInfo.collider.transform.parent.GetComponent<Tile>());
    }

    List<Tile> hoverList = new List<Tile>();
    List<Tile> prevHoverList = new List<Tile>();
    Tile prevHitTile;
    void DrawMousePath()
    {
        Map map = Game.World.GetComponent<Map>();

        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Tile hitTile = hitInfo.collider.transform.parent.GetComponent<Tile>();
            hoverList = map.AStar(map.currentTile, hitTile);
            if (hitTile != prevHitTile)
            {
                foreach (Tile tile in prevHoverList)
                    tile.SetColor(tile.tileType.defaultColor);
                foreach (Tile tile in hoverList)
                    tile.SetColor(tile.tileType.hoverColor);

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
