using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("click");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
            {
                Debug.Log("hit");
                if (hit.collider.gameObject.tag == "Walkable")
                {
                    agent.destination = hit.point;
                }
            }
        }
    }
}
