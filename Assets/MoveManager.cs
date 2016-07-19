using UnityEngine;
using System.Collections;

public class MoveManager : MonoBehaviour {

	public bool selectingTile = false;
	public GameObject player;
	public float fract;
	private bool shouldMove = false;
	private Vector3 gotolocation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
		if (shouldMove) {
			player.transform.position = Vector3.MoveTowards(player.transform.position, gotolocation, fract);
		}
	}

	public void moveTo (Vector3 location){
		if (selectingTile) {
			shouldMove = true;
			gotolocation = location;
		}
	}
}
