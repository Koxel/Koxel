using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUnload : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    void OnBecameVisible()
    {
        gameObject.SetActive(true);
    }
}
