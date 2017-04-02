using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

    public GameObject followThis;
    public float followSpeed;
    bool following;

	void Update () {
		if(followThis != null)
        {
            float interpolation = followSpeed * Time.deltaTime;

            Vector3 position = transform.position;
            position.x = Mathf.Lerp(transform.position.x, followThis.transform.position.x, interpolation);
            position.y = Mathf.Lerp(transform.position.y, followThis.transform.position.y, interpolation);
            position.z = Mathf.Lerp(transform.position.z, followThis.transform.position.z, interpolation);

            transform.position = position;
        }
	}
}
