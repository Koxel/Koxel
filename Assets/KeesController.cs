using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeesController : MonoBehaviour {

    Animator animator;
    Rigidbody rigid;
    bool grounded;
    float orgGroundedDist;
    Vector3 m_GroundNormal;
    Vector3 move;

    public Transform cam;
    public float moveSpeed = 2f;
    public float jumpForce = 2f;
    public float groundedDistance = .1f;
    public float gravityMultiplier = 2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        orgGroundedDist = groundedDistance;

        if (cam == null)
            cam = Camera.main.transform;

        if (IsGrounded())
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        move = ver * camForward + hor * cam.right;
        //move.y = rigid.velocity.y;
        move *= moveSpeed;

        
    }

    private void FixedUpdate()
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        move.y = rigid.velocity.y;
        rigid.velocity = move;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            grounded = false;
            rigid.AddForce(new Vector3(0, jumpForce, 0));
        }

        if (!grounded)
        {
            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
            rigid.AddForce(extraGravityForce);

            groundedDistance = rigid.velocity.y < 0 ? orgGroundedDist : 0.01f;

            if (IsGrounded())
            {
                grounded = true;
            }
        }
    }

    bool IsGrounded()
    {
        RaycastHit hitInfo;

        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundedDistance))
        {
            m_GroundNormal = hitInfo.normal;
            return true;
        }
        else
        {
            m_GroundNormal = Vector3.up;
            return false;
        }
    }

}
