using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Koxel;

public class TileCoordsText : MonoBehaviour {

    TextMesh textMesh;
    Tile tile; 

	void Start () {
        if (!Game.instance.gameConfig.DEBUG)
            Destroy(gameObject);
        textMesh = GetComponent<TextMesh>();
        tile = GetComponentInParent<Tile>();
	}
	
	void Update () {
        if (textMesh.text != tile.coords.x + "," + tile.coords.y)
            textMesh.text = tile.coords.x + "," + tile.coords.y;
    }
}
