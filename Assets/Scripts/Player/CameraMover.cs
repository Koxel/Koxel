using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    public Transform target;
    public float followSpeed = 2f;

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

    private void Update()
    {
        if (target != null)
        {

            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * followSpeed);


        }
    }




    /*public float moveSpeed = 5f;
    
    void Update()
    {
        if (!World.instance.chunks.ContainsKey(new Vector3()))
            return;
        Vector3 translate = new Vector3
            (
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            );

        this.transform.position = Vector3.Lerp(transform.position, transform.position + (translate * 2 * (1 + this.transform.position.y / 2)), moveSpeed * Time.deltaTime);

        Update_Rotation();
        Update_ScrollZoom();
    }

    void Update_Rotation()
    {
        
    }

    // Original code from https://github.com/quill18/MostlyCivilizedHexEngine/blob/master/Assets/Scripts/MouseController.cs
    Vector3 cameraTargetOffset;
    void Update_ScrollZoom()
    {
        // Zoom to scrollwheel
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        if (scrollAmount > 0f)
            scrollAmount = .1f;
        else if (scrollAmount < 0f)
            scrollAmount = -.1f;

        float minHeight = 7 + World.instance.chunks[new Vector3()].tiles[new Vector3()].transform.position.y;
        float maxHeight = 20;
        // Move camera towards hitPos
        Vector3 hitPos = this.transform.position - new Vector3(0, 0, scrollAmount * 17f);
        Vector3 dir = hitPos - Camera.main.transform.position;

        Vector3 p = Camera.main.transform.position;

        // Stop zooming out at a certain distance.
        // TODO: Maybe you should still slide around at 20 zoom?
        if (scrollAmount > 0 || p.y < (maxHeight - 0.1f))
        {
            cameraTargetOffset += dir * scrollAmount;
        }
        Vector3 lastCameraPosition = Camera.main.transform.position;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + cameraTargetOffset, Time.deltaTime * 5f);
        cameraTargetOffset -= Camera.main.transform.position - lastCameraPosition;


        p = Camera.main.transform.position;
        if (p.y < minHeight)
        {
            p.y = minHeight;
        }
        if (p.y > maxHeight)
        {
            p.y = maxHeight;
        }
        Camera.main.transform.position = p;

        // Change camera angle
        Camera.main.transform.rotation = Quaternion.Euler(
            Mathf.Lerp(30, 75, Camera.main.transform.position.y / maxHeight),
            Camera.main.transform.rotation.eulerAngles.y,
            Camera.main.transform.rotation.eulerAngles.z
        );
    }*/
}
