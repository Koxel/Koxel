using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour {

    public float speed = 5f;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 1, 0));
    }
}
