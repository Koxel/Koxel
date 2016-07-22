using UnityEngine;
using System.Collections;

public class MoveManager : MonoBehaviour {

	public bool selectingTile;
	public GameObject player;
	private float fract = 0.1f;
	private bool shouldMove = false;
	private Vector3 gotolocation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
		if (shouldMove){
			player.transform.position = Vector3.MoveTowards (player.transform.position, gotolocation, fract);

			if (Vector3.Distance (player.transform.position, gotolocation) == 0) { 
				shouldMove = false;
				gameObject.GetComponent<TileTicker> ().TileTick ();
			}
		}

	}

	public void moveTo (Vector3 location) {
		if (selectingTile) {
			// Allow movement ..
			shouldMove = true;
			// .. to the given location
			gotolocation = location;
		}
	}
}
