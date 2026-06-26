//this script is used with the material selection panels. It drags and drops an object
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;

    Vector3 startPosition;
    Transform startParent;
    Canvas thisCanvas;

    void Start()
    {
        thisCanvas = FindObjectOfType<Canvas>();
    }


    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {

        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        //        throw new System.NotImplementedException();
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        //this works for Canvas with Overlay selected
        if(thisCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
          transform.position = Input.mousePosition;

        //this group works with Canvas with Camera selected and Identified is selected
        if (thisCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            transform.position = Input.mousePosition;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 1.0f;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }

    }

    #endregion

    #region EndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent == startParent)
        {
              transform.position = startPosition;
        }


        //        throw new System.NotImplementedException();
    }
    #endregion

}
