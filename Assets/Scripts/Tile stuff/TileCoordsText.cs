using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Koxel;

public class TileCoordsText : MonoBehaviour {

    TextMesh textMesh;
    HexCalc hexCalc;
    HexData hexData;
    Tile tile; 

	void Start () {
        if (!Game.instance.gameConfig.DEBUG)
            Destroy(gameObject);
        hexCalc = new HexCalc();
        hexData = new HexData(Game.instance.gameConfig.hexSize);
        textMesh = GetComponent<TextMesh>();
        tile = GetComponentInParent<Tile>();
	}
	
	void Update () {
        if (textMesh.text != tile.coords.x + "," + tile.coords.y)
            textMesh.text = tile.coords.x + "," + tile.coords.y;
    }
}
