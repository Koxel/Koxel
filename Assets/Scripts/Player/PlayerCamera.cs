using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public static PlayerCamera instance;

    public Transform target;
    public float followSpeed = 2f;

    [Header("Rotation")]
    public float rotXSpeed = 120f;
    public float rotYSpeed = 120f;
    public Vector2 rotLimits = new Vector2(-20f, 80f);
    public float rotLerpSpeed = 2f;
    float x = 0.0f;
    float y = 0.0f;

    [Header("Zooming")]
    public float zoomSpeed = 1f;
    public Vector2 zoomLimits = new Vector2(1, 10);

    private void Awake()
    {
        instance = this;
    }

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
        
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        // Position
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * followSpeed);

        //Rotation
        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * rotXSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * rotYSpeed * 0.02f;

            y = ClampAngle(y, rotLimits.x, rotLimits.y);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            transform.rotation = rotation;
        }

        //Zoom
        float scroll = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        Vector3 scale = transform.localScale;
        float scaler = Mathf.Clamp(scale.x + scroll, zoomLimits.x, zoomLimits.y);
        transform.localScale = new Vector3(scaler, scaler, scaler);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void ColliderHit(Quaternion rot)
    {
        transform.rotation = rot;
    }
}
