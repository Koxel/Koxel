using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {

    public Tile Tile;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 coordinates { get; set; }

    public virtual bool IsWalkable()
    {
        return true;
    }
    public virtual float MovementCost()
    {
        return 0;
    }
}

