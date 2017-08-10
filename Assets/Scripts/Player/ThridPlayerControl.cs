using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPlayerControl : MonoBehaviour {

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;

    public float turnSpeed = 1f;

    // Use this for initialization
    void Start () {
        m_Cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }

        Move(m_Move);
    }

    private void Move(Vector3 move)
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);

        //Rotate
        transform.Rotate(0, Mathf.Atan2(move.x, move.z) * Time.deltaTime * turnSpeed, 0);
    }
}
