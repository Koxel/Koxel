using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveManager : MonoBehaviour {

	public bool selectingTile = true;
	public GameObject player;
	private float fract = 0.1f;
	private bool shouldMove = false;
	private Transform MoveGoal;
	private List<Transform> TilePath;
	private int pathItem;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
		if (shouldMove){
			//Debug.Log (TilePath [pathItem]);
			player.transform.position = Vector3.MoveTowards (player.transform.position, TilePath[pathItem].position, fract);

			if (Vector3.Distance (player.transform.position, TilePath [pathItem].position) == 0) {
				gameObject.GetComponent<TileTicker> ().TileTick ();

				if (pathItem != 0)
					pathItem = pathItem - 1;
				else
					shouldMove = false;
				
			}
				
				
		}
	}

	public void moveTo (Transform goal) {
		if (selectingTile) {
			TilePath = CalculatePath (goal);
			pathItem = TilePath.Count-1;
			shouldMove = true;

		}
	}

	public List<Transform> CalculatePath (Transform goal) {
		List<Transform> path = new List<Transform>(); // use reversed, first one in list is last tile.
		path.Add(goal);

		while (Vector3.Distance (player.transform.position, goal.position) > 2f) {
			Transform closestTile = GetClosestTile (goal.GetComponent<Tile> ().FindConnectedTiles ());

			if (closestTile != null)
			path.Add (closestTile);
			goal = closestTile;
		}

		return path;
	}

	public Transform GetClosestTile(List<Transform> tiles){
		Transform tMin = null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = player.transform.position;

		foreach (Transform tile in tiles) {
			float dist = Vector3.Distance (tile.position, currentPos);
			if (dist < minDist) {
				tMin = tile;
				minDist = dist;
			}
		}

		return tMin;
	}

	void NextTile() {
		if (shouldMove) {

		}
	}
}
