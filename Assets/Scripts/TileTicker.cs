using UnityEngine;
using System.Collections;

public class TileTicker : MonoBehaviour {

	public float tickRange = 3f;

	public void TileTick (Vector3 pos) {

		foreach (Transform child in transform)
		{
			if (Vector3.Distance(child.position, pos) <= tickRange * 2f)
				child.GetComponent<Tile> ().TileTick ();
		}
	}
}
