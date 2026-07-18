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
        //thisCanvas = FindAnyObjectByType<Canvas>();
        thisCanvas = FindAnyObjectByType<Canvas>();
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
        if (thisCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            transform.position = eventData.position;
        }

        if (thisCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 pointerPosition = eventData.position;
            pointerPosition.z = 1.0f;

            Camera eventCamera = eventData.pressEventCamera;

            if (eventCamera != null)
            {
                transform.position =
                    eventCamera.ScreenToWorldPoint(pointerPosition);
            }
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
