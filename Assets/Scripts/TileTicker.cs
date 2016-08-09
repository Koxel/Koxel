using UnityEngine;
using System.Collections;

public class TileTicker : MonoBehaviour {
	
	public void TileTick () {
		Debug.Log ("Tick");

		foreach (Transform child in transform)
		{
			child.GetComponent<Tile> ().TileTick ();
		}
	}
}
