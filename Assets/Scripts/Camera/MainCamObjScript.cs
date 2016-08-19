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
    public float movspeed = 0.5f;
    public static int i = 0;
	void Update () {

        if (Input.GetMouseButton(1)) {
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.Rotate(Vector3.forward, rotspeed);
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.Rotate(Vector3.forward, -1 * rotspeed);
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse Y") > 0 && i < maxheight)
            {
                transform.Translate(new Vector3(0, 0, 1) * movspeed);
                i++;
            }
            if (Input.GetAxis("Mouse Y") < 0 && i > maxheight * -1)
            {
                transform.Translate(new Vector3(0, 0, -1) * movspeed);
                i--;
            }
        }
    }
}
