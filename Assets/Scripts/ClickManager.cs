using UnityEngine;
using System.Collections;

public class ClickManager : MonoBehaviour {

    public GameObject map;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log("hit: " + hitInfo.collider.gameObject.GetComponentInParent<Hex>().x + ", " + hitInfo.collider.GetComponentInParent<Hex>().y);

                map.GetComponent<TileMap>().CalculatePath(hitInfo.collider.gameObject.GetComponentInParent<Hex>().x, hitInfo.collider.GetComponentInParent<Hex>().y);
            }


        }

	}
}
