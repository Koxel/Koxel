using UnityEngine;
using System.Collections;

//
//  This gets loaded on "Main Camera"
//

public class LegitCameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public float movespeed = 5f;
    public int max = 5;
    public static int i = 0;

    // Update is called once per frame
    void Update () {

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && i < max)
        {
            transform.Translate(Vector3.forward * movespeed);
            i++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && i > max * -1)
        {
            transform.Translate(Vector3.back * movespeed);
            i--;
        }
    }
}
