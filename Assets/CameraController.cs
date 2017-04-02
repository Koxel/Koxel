using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Game Game;
    Camera spectateCam;
    Camera playerCam;

	// Use this for initialization
	public void Setup (GameObject player) {
        Game = GetComponent<Game>();

        spectateCam = transform.FindChild("Spectator Camera").transform.GetChild(0).GetComponent<Camera>();
        playerCam = transform.FindChild("Player Camera").transform.GetChild(0).GetComponent<Camera>();

        spectateCam.enabled = false;
        spectateCam.GetComponent<AudioListener>().enabled = false;
        spectateCam.GetComponentInParent<Spectator>().spectating = false;

        playerCam.enabled = true;
        playerCam.GetComponent<AudioListener>().enabled = true;

        playerCam.transform.parent.GetComponent<Follower>().followThis = player;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Switch Camera
            spectateCam.enabled = !spectateCam.enabled;
            spectateCam.GetComponent<AudioListener>().enabled = spectateCam.enabled;
            spectateCam.GetComponentInParent<Spectator>().spectating = spectateCam.enabled;

            playerCam.enabled = !playerCam.enabled;
            playerCam.GetComponent<AudioListener>().enabled = playerCam.enabled;
        }
        if (playerCam.enabled)
        {
            if(!Game.World.GetComponent<Map>().moving)
                DrawMousePath();

            if (Input.GetMouseButtonDown(0))
                CreatePath();
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
