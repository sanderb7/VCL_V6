//used to set the orientation of the material principle directions in the Build Directional Sample Scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAxis : MonoBehaviour
{
    private GetLayerProperties layerProperties;

    private void Start()
    {
        layerProperties = FindObjectOfType<GetLayerProperties>();
    }

    void LateUpdate()
    {
        this.transform.eulerAngles = new Vector3(0, 0, -layerProperties.layerOrientation);
    }
}
