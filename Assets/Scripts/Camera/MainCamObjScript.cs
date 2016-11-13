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
    public int maxheight = 7;
    public static int i = 0;
	void Update () {

        if (Input.GetMouseButton(1)) {
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.RotateAround(transform.position, Vector3.up, rotspeed);
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.RotateAround(transform.position, Vector3.down, rotspeed);
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse Y") > 0)
            {
                transform.Rotate(Vector3.right, rotspeed);
                i++;
            }
            if (Input.GetAxis("Mouse Y") < 0)
            {
                transform.Rotate(Vector3.left, rotspeed);
                i--;
            }
        }
    }
}
