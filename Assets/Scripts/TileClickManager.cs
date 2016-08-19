using UnityEngine;
using System.Collections;
using System;

public class TileClickManager : MonoBehaviour {

	public MoveManager manager;

	void OnMouseDown(){
        // Start coroutine waiting for 0.1 sec
        StartCoroutine(countclick());
	}

    private IEnumerator countclick()
    {
        yield return new WaitForSeconds(0.1f);
        // If mouse is NOT still down: count as a legit click!
        if (!Input.GetMouseButton(0))
        {
            manager.moveTo(transform);
        }
    }
}