using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    Transform cam;
    CharacterController controller;
    float verticalSpeed = 0f;

    public float moveSpeed = 2f;
    public float turnSpeed = 90f;
    public float gravity = 9.81f;
    public float jumpSpeed = 5f;

	void Awake ()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
	}

    void Update()
    {
        if (!MousePointer.instance.menuOpen) {
            Vector3 forward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1).normalized);
            Vector3 move = Input.GetAxis("Vertical") * forward + Input.GetAxis("Horizontal") * cam.right;
            Vector3 mover = move;

            if (move.magnitude > 1f) move.Normalize();
            move = transform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, Vector3.up);

            float turnAmount = Mathf.Atan2(move.x, move.z);
            float turnSpeed = Mathf.Lerp(180, 360, move.z);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);

            //transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
            //Vector3 move = transform.forward * Input.GetAxis("Vertical") * moveSpeed;
            //Vector3 move = new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0f, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);

            if (controller.isGrounded)
            {
                verticalSpeed = 0;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    verticalSpeed = jumpSpeed;
                }
            }

            mover *= moveSpeed;
            verticalSpeed -= gravity * Time.deltaTime;
            mover.y = verticalSpeed;

            controller.Move(mover * Time.deltaTime);
        }
    }
}
