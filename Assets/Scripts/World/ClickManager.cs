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
            Debug.DrawRay(ray.origin, ray.direction * 5000, Color.cyan, 5f);

            if (Physics.Raycast(ray, out hitInfo))
            {
                //Debug.Log("hit: " + hitInfo.collider.transform.parent.name);
                //GetComponent<Map>().PathTo(hitInfo.collider.transform.parent.GetComponent<TileBehaviour>());
                //GetComponent<Map>().PixelPath(hitInfo.collider.transform.parent.GetComponent<TileBehaviour>());
                GetComponent<Map>().PixelPath(hitInfo.collider.transform.parent.GetComponent<TileBehaviour>());
            }
        }
    }
}