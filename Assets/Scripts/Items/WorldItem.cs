using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour {

    public Item item;
    private Rigidbody rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
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