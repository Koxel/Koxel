using UnityEngine;
using System.Collections;
using System;

public class TileClickManager : MonoBehaviour {

	public MoveManager manager;

	void OnMouseDown(){
        manager.moveTo(transform);

    }
}