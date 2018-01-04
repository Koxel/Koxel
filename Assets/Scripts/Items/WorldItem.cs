using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour {

    public Item item;
    private Rigidbody rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        /*Vector3 sideDir = new Vector3();
        if(Random.Range(0, 2) == 1)
        {
            sideDir = transform.right;
        }
        else
        {
            sideDir = transform.forward;
        }
        if(Random.Range(0, 2) == 1)
        {
            sideDir *= -1;
        }*/
        rb.AddRelativeForce(transform.up * 250f + Random.onUnitSphere * 100f);
	}

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 1, 0));
    }

    public void PickUp(Player player)
    {
        player.inventory.Add(item);
        Destroy(gameObject);
    }
}