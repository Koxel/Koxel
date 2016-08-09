using UnityEngine;
using System.Collections;

public class getdistance : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Transform parent = transform.parent;

		foreach (Transform tile in parent.transform){
			//Debug.Log (tile.name + Vector3.Distance (transform.position, tile.position));
			if (tile != transform) 
				if (Vector3.Distance (transform.position, tile.position) < 2f)
					Debug.Log (tile.name + Vector3.Distance (transform.position, tile.position));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
