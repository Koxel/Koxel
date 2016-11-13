using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveManager : MonoBehaviour {

	private bool selectingTile = true;
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
			player.transform.position = Vector3.MoveTowards (player.transform.position, MoveGoal.position, fract);

			if (Vector3.Distance (player.transform.position, TilePath [pathItem].position) == 0)
				NextTile ();
		}
	}

	public void NextTile(){
		gameObject.GetComponent<TileTicker> ().TileTick (player.transform.position);

		if (pathItem != 0){
			pathItem = pathItem - 1;
			MoveGoal = TilePath [pathItem];
		} else {
			shouldMove = false;
			selectingTile = true;
		}
	}

	public void moveTo (Transform goal) {
		if (selectingTile) {
			TilePath = CalculatePath (goal);
			pathItem = TilePath.Count-1;
			MoveGoal = TilePath [pathItem];
			shouldMove = true;
			selectingTile = false;
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
}
