﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RotateToCamera : MonoBehaviour {
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles);
    }
    void Start()
    {
        transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles);
    }
}
