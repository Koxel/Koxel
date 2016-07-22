using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileTicker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TileTick () {
		Debug.Log ("Tick");

		foreach (Transform child in transform)
		{
			child.GetComponent<Tile> ().TileTick ();
		}
	}
}
