using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour {

    public GameObject pointer;
    public Vector3 pointerOffset = new Vector3(0f, .1f, 0f);
    public float mouseDistance = 2f;
    private Animation menuAnim;
    private bool menuOpen;
    private Transform selectedObject;
    private Vector3 defaultScale;
    private GameObject cursorObject;
    public GameObject RadialMenu;
    public GameObject MiddleHex;
    public GameObject OuterHex;
    public Vector3 outerHexRotation;

    void Start() {
        cursorObject = pointer.transform.GetChild(0).gameObject;
        menuAnim = cursorObject.GetComponentInChildren<Animation>();
        defaultScale = pointer.transform.localScale;
    }

    void Update() {
        RaycastInfo raycast = MouseRaycast();
        if (raycast.hit)
        {
            if (menuOpen == false) {
                if (selectedObject == null)
                {
                    if (!cursorObject.activeSelf)
                        cursorObject.SetActive(true);

                    pointer.transform.position = raycast.rayHit.point + pointerOffset;
                }
                else
                {
                    if (Vector3.Distance(pointer.transform.position, raycast.rayHit.point + pointerOffset) >= mouseDistance && selectedObject != null)
                    {
                        UnSelectObject();
                        pointer.transform.position = raycast.rayHit.point + pointerOffset;
                    }
                    else if (selectedObject != raycast.rayHit.collider.transform && !raycast.rayHit.collider.CompareTag("Ground"))
                    {
                        SelectObject(raycast.rayHit.collider.transform);
                    }
                }
            }
        }
        else
        {
            if (selectedObject == null)
            {
                if (cursorObject.activeSelf)
                    cursorObject.SetActive(false);
            }
        }

        //LClick OpenMenu animation
        if (selectedObject != null) {
            if (Input.GetMouseButtonDown(0) && !menuAnim.isPlaying)
            {
                //Toggle menu
                if (menuOpen)
                {
                    StartCoroutine(RadialMenuAnimation(-1));
                }
                else
                {
                    StartCoroutine(RadialMenuAnimation(1));
                }
            }
        }
    }

    private void FixedUpdate()
    {
        OuterHex.transform.Rotate(outerHexRotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!cursorObject.activeSelf)
            return;
        if (other.CompareTag("Ground"))
        {
            if (selectedObject != null)
                UnSelectObject();
            return;
        }

        if (selectedObject != other.transform)
        {
            RaycastInfo raycast = MouseRaycast();
            if (raycast.hit)
            {
                if (!raycast.rayHit.collider.CompareTag("Ground"))
                {
                    if (MouseRaycast().rayHit.collider.transform == other.transform)
                    {
                        SelectObject(other.transform);
                        return;
                    }
                }
            }
            SelectObject(other.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (selectedObject == null)
        {
            OnTriggerEnter(other);
        }
    }

    void SelectObject(Transform target)
    {
        selectedObject = target;
        MiddleHex.SetActive(false);
        RadialMenu.SetActive(false);
        pointer.transform.position = selectedObject.position + pointerOffset;
        pointer.transform.localScale = defaultScale + (selectedObject.localScale - new Vector3(1, 1, 1));
    }

    void UnSelectObject()
    {
        selectedObject = null;
        if (!cursorObject.activeSelf)
            cursorObject.SetActive(true);
        MiddleHex.SetActive(true);
    }

    IEnumerator RadialMenuAnimation(float speed)
    {
        if (speed > 0)
        {
            menuOpen = true;
            RadialMenu.SetActive(true);
            menuAnim["OpenMenu"].speed = speed;
            menuAnim.Play("OpenMenu");
        }
        else
        {
            menuAnim["OpenMenu"].speed = speed;
            menuAnim["OpenMenu"].time = menuAnim["OpenMenu"].length;
            menuAnim.Play("OpenMenu");

            yield return new WaitForSeconds(menuAnim["OpenMenu"].length);

            menuOpen = false;
            RadialMenu.SetActive(false);
        }
    }

    struct RaycastInfo
    {
        public readonly RaycastHit rayHit;
        public readonly bool hit;

        public RaycastInfo(RaycastHit rayHit, bool hit)
        {
            this.rayHit = rayHit;
            this.hit = hit;
        }
    }

    RaycastInfo MouseRaycast(int layermask = -1)
    {
        bool hitsomething;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (layermask != -1) {
            if (Physics.Raycast(ray, out hit, layermask))
                hitsomething = true;
            else
                hitsomething = false;
        }
        else
        {
            if (Physics.Raycast(ray, out hit))
                hitsomething = true;
            else
                hitsomething = false;
        }
        return new RaycastInfo(hit, hitsomething);
    }
}
