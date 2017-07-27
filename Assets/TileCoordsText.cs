using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCoordsText : MonoBehaviour {

    TextMesh textMesh;

	// Use this for initialization
	void Start () {
        if (!Game.instance.gameConfig.DEBUG)
            Destroy(gameObject);

        textMesh = GetComponent<TextMesh>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (textMesh.text != transform.parent.name.Replace("Tile", "").Replace(" ", "").Replace("(", "").Replace(")", ""))
            textMesh.text = transform.parent.name.Replace("Tile", "").Replace(" ", "").Replace("(", "").Replace(")", "");
    }
}
