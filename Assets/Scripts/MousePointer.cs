using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour {

    public static MousePointer instance;

    public GameObject pointer;
    public Vector3 pointerOffset = new Vector3(0f, .1f, 0f);
    public float mouseDistance = 2f;
    public Animation menuAnim;
    public bool menuOpen;
    private Transform selectedObject;
    private Vector3 defaultScale;
    private GameObject cursorObject;
    public GameObject RadialMenu;
    public GameObject MiddleHex;
    public GameObject OuterHex;
    public Vector3 outerHexRotation;
    public List<string> selectableTags = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    void Start() {
        cursorObject = pointer.transform.GetChild(0).gameObject;
        /*menuAnim = cursorObject.GetComponentInChildren<Animation>();*/
        defaultScale = pointer.transform.localScale;
    }

    void Update() {
        RaycastInfo raycast = MouseRaycast();
        if (raycast.hit)
        {
            if (menuOpen == false)
            {
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
                    //Close
                    StartCoroutine(RadialMenuAnimation(-1, new Dictionary<int, Asset_Interaction>()));
                }
                else
                {
                    //Open
                    Dictionary<int, Asset_Interaction> cells = new Dictionary<int, Asset_Interaction>();
                    if (selectedObject.CompareTag("TileAssetModel"))
                    {
                        if (selectedObject.parent.parent.GetComponent<TileAsset>().assetInteractions != null)
                        {
                            List<Asset_Interaction> myList = selectedObject.parent.parent.GetComponent<TileAsset>().assetInteractions;
                            foreach (Asset_Interaction AI in myList)
                            {
                                int index = myList.IndexOf(AI);
                                cells.Add(index, AI);
                                Debug.Log(AI.name);
                            }
                        }
                    }
                    StartCoroutine(RadialMenuAnimation(1, cells));
                }
            }
        }
    }

    private void FixedUpdate()
    {
        OuterHex.transform.Rotate(outerHexRotation * Time.deltaTime);
        /*Quaternion hexRot = MiddleHex.transform.localRotation;
        hexRot.y = Camera.main.transform.parent.localRotation.y;
        MiddleHex.transform.localRotation = hexRot;*/
        MiddleHex.transform.eulerAngles = new Vector3(MiddleHex.transform.eulerAngles.x, Camera.main.transform.parent.eulerAngles.y, MiddleHex.transform.eulerAngles.z);
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
                if (raycast.rayHit.collider.GetComponent<SelectionTarget>() != null)
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
        pointer.transform.SetParent(target);
        pointer.transform.position = selectedObject.position + pointerOffset;
    }

    void UnSelectObject()
    {
        selectedObject = null;
        pointer.transform.SetParent(null);
        pointer.transform.localScale = defaultScale;
        if (!cursorObject.activeSelf)
            cursorObject.SetActive(true);
        MiddleHex.SetActive(true);
    }

    IEnumerator RadialMenuAnimation(float speed, Dictionary<int, Asset_Interaction> cells)
    {
        if (speed > 0)
        {
            menuOpen = true;
            RadialMenu.SetActive(true);
            foreach(int i in cells.Keys)
            {
                GameObject option = RadialMenu.transform.GetChild(i).gameObject;
                option.SetActive(true);
                AssetInteraction AI = option.GetComponent<AssetInteraction>();
                AI.Setup(cells[i]);
            }

            menuAnim["OpenMenu"].speed = speed;
            menuAnim.Play("OpenMenu");
        }
        else
        {
            menuAnim["OpenMenu"].speed = speed;
            menuAnim["OpenMenu"].time = menuAnim["OpenMenu"].length;
            menuAnim.Play("OpenMenu");

            yield return new WaitForSeconds(menuAnim["OpenMenu"].length);

            foreach (Transform child in RadialMenu.transform)
                child.gameObject.SetActive(false);
            RadialMenu.SetActive(false);
            menuOpen = false;
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
