using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour {

    public bool spectating;
	
	void Update () {
        if (spectating)
        {
            Vector3 pos = transform.position;
            float interpolation = 10f * Time.deltaTime;
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                pos.x = Mathf.Lerp(pos.x, pos.x + Input.GetAxis("Horizontal") * 2, interpolation);
                pos.z = Mathf.Lerp(pos.z, pos.z + Input.GetAxis("Vertical") * 2, interpolation);
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit))
                pos.y = Mathf.Lerp(pos.y, hit.transform.position.y + 15f, interpolation);

            transform.position = pos;
        }
    }
}
