using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour {
    public Map map;
    public GameObject player;
    public Vector3 offset;
    public float speed = 2;
    public bool follow = false;

    void Start()
    {
        map.playerCam = gameObject.GetComponent<PlayerCam>();
        offset = transform.position;
    }
    
    void Update()
    {
        if (!follow)
        {
            if (player == null)
                return;

            float interpolation = speed * Time.deltaTime;

            Vector3 position = this.transform.position;
            position.x = Mathf.Lerp(transform.position.x, player.transform.position.x + offset.x, interpolation);
            position.y = Mathf.Lerp(transform.position.y, player.transform.position.y + offset.y, interpolation);
            position.z = Mathf.Lerp(transform.position.z, player.transform.position.z + offset.z, interpolation);

            transform.position = position;
        }
    }

    public void SetPlayer(GameObject player)
    {
        offset = transform.position;
        this.player = player;
    }
}
