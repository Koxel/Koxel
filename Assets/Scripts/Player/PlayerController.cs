using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    private bool movementEnabled;

    Transform cam;
    CharacterController controller;
    Player player;
    float verticalSpeed = 0f;
    public GameObject InteractSprite;

    Interactable interactable;
    GameObject InteractObject;
    public float moveSpeed = 2f;
    public float turnSpeed = 90f;
    public float gravity = 9.81f;
    public float jumpSpeed = 5f;
    public float sprintSpeed = 3.5f;

    Vector3 move;
    Vector3 mover;

    void Awake ()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        player = GetComponent<Player>();
	}

    //Interaction box draw
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + new Vector3(0f, .2f) + transform.forward * 1f, .5f);
    }

    private void Update()
    {
        CheckInteractions();

        if (Input.GetKeyDown(KeyCode.Mouse0) && InteractObject != null && movementEnabled)
        {
            AssetInteraction ai = InteractObject.GetComponentInChildren<InteractionMenu>().options[0];
            ai.Activate(interactable, player);
            Destroy(InteractObject);
        }

        CalculateMovement();
    }

    private void FixedUpdate()
    {
        float turnAmount = Mathf.Atan2(move.x, move.z);
        float turnSpeed = Mathf.Lerp(180, 360, move.z);

        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        controller.Move(mover * Time.deltaTime);
    }

    void CheckInteractions()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        /*Collider[] found = Physics.OverlapCapsule(
            transform.position + transform.forward * 1.2f, 
            transform.position + new Vector3(0f, 1f) + transform.forward * 1.2f, 
            .5f, layerMask);*/
        Collider[] found = Physics.OverlapSphere(transform.position + new Vector3(0f, .2f) + transform.forward * 1f, .5f, layerMask);
        if (found.Length != 0)
        {
            /*foreach (Collider obj in found)
            {
                Debug.Log(obj.gameObject);
            }*/
            GameObject thing = found[0].gameObject;
            if (thing.GetComponent<Interactable>() != null)
            {
                if (thing.GetComponent<Interactable>() != interactable)
                {
                    interactable = null;
                    if (InteractObject != null)
                        Destroy(InteractObject);

                    interactable = thing.GetComponent<Interactable>();
                    if (interactable.assetInteractions.Count > 0)
                    {
                        InteractObject = Instantiate(InteractSprite, interactable.transform.GetChild(0).position, Quaternion.identity);
                        InteractObject.GetComponent<InteractionMenu>().Setup(interactable.assetInteractions);
                    }
                }
            }
            else if (thing.GetComponentInParent<Interactable>() != null)
            {
                if (thing.GetComponentInParent<Interactable>() != interactable)
                {
                    interactable = null;
                    if (InteractObject != null)
                        Destroy(InteractObject);

                    interactable = thing.GetComponentInParent<Interactable>();
                    if (interactable.assetInteractions.Count > 0)
                    {
                        InteractObject = Instantiate(InteractSprite, interactable.transform.GetChild(0).position, Quaternion.identity);
                        InteractObject.GetComponentInChildren<InteractionMenu>().Setup(interactable.assetInteractions);
                    }
                }
            }
        }
        else
        {
            interactable = null;
            if (InteractObject != null)
                Destroy(InteractObject);
        }
    }

    void CalculateMovement()
    {
        Vector3 forward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1).normalized);

        float horIn = Input.GetAxis("Horizontal");
        float verIn = Input.GetAxis("Vertical");

        if (!movementEnabled)
        {
            horIn = 0f;
            verIn = 0f;
        }

        move = verIn * forward + horIn * cam.right;
        mover = move;

        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, Vector3.up);

        if (controller.isGrounded)
        {
            verticalSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalSpeed = jumpSpeed;
            }
        }

        //mover.Normalize();
        if (Input.GetButton("Sprint"))
            mover *= sprintSpeed;
        else
            mover *= moveSpeed;
        verticalSpeed -= gravity * Time.deltaTime;
        mover.y = verticalSpeed;
    }    

    public void EnableMovement()
    {
        movementEnabled = true;
    }
    public void DisableMovement()
    {
        movementEnabled = false;
    }
}
