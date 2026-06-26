using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientChildren : MonoBehaviour
{
    private void LateUpdate()
    {
        foreach (Transform child in transform)
        {
            child.transform.localPosition = new Vector3(child.transform.localPosition.x, child.transform.localPosition.y, 0.0f);
        }
    }
}
