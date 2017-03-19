using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    public GameObject map;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            //Debug.DrawRay(ray.origin, ray.direction * 5000, Color.cyan, 5f);

            if (Physics.Raycast(ray, out hitInfo))
            {
                GetComponent<Map>().PixelPath(hitInfo.collider.transform.parent.GetComponent<Tile>());
            }
        }
    }
}