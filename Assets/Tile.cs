using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public string type;
	public Vector3 playerLocation = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
		playerLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
