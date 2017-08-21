using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public Transform target;
    public float followSpeed = 2f;
    public float rotateSpeed =2f;
    public KeyCode rotL;
    public KeyCode rotR;

    private void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * followSpeed);
        }

        float rot = 0f;
        if (Input.GetKey(rotL))
        {
            rot -= 1f;
        }
        if (Input.GetKey(rotR))
        {
            rot += 1f;
        }

        transform.Rotate(0, rot*rotateSpeed, 0);
    }
}
