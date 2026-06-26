//this script is used to rotate the tensile specimen in the non-directional material design scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{ 
    private float rotationSpeed = 0.1f; //degress/sec

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(rotationSpeed * Vector3.up);
    }
}
