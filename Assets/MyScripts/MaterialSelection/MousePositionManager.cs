//I don't think this is used anymore.  keep it around since it may be useful
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionManager : MonoBehaviour
{
    public Transform designCanvas;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = -Vector3.one;
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(designCanvas.position.x, 0, 0));
            Debug.Log(mousePosition);           
        }
    }
    

}
