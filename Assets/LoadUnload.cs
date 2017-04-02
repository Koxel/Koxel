using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUnload : MonoBehaviour {

    GameObject loader;
    GameObject currentChunk;
    float radius = 10f;

    void Start()
    {
        loader = GameObject.Find("Spectator Camera");
    }

    void Update()
    {
        //GetCurrentChunk();
    }

    void GetCurrentChunk()
    {
        Vector3 pos = loader.transform.position;

        currentChunk = loader.GetComponent<Controls>().map.currentTile.transform.parent.gameObject;


    }
    //Distance between neighbours = ~27.71;
}
