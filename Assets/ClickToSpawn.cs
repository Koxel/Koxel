using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToSpawn : MonoBehaviour {

    public GameObject prefab;
    private GameObject clone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //if (clone == null)
            //{
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    clone = Instantiate(prefab, hit.point + Vector3.up, Quaternion.identity);
                }
                
            //}
            //else
                //Destroy(clone);
        }
	}
}
