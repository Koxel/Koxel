using UnityEngine;
using System.Collections;

public class TileClickManager : MonoBehaviour {

	public MoveManager manager;

	void OnMouseDown(){
		manager.moveTo (transform.position);
	}
}
