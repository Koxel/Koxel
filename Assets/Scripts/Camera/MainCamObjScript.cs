using UnityEngine;
using System.Collections;

//
//  This gets loaded onto "maincamobj"
//

public class MainCamObjScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    public float rotspeed = 5f;
	void Update () {
        if (Input.GetMouseButton(0))
        {
            if(Input.GetAxis("Mouse X") > 0)
            {
                transform.Rotate(Vector3.forward, rotspeed);
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.Rotate(Vector3.forward, -1 * rotspeed);
            }
        }
	}
}
