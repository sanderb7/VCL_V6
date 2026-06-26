//used once a material is settled into a material slot
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IDropHandler
{
    private DataController dataController;
    private GetLayerProperties laminateLayers;

    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        laminateLayers = FindObjectOfType<GetLayerProperties>();
    }

    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }


    #region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {

        if (Item)
        {
            Destroy(Item);
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }
        else
        {
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }
        SetLayerMaterial();

    }
    #endregion
    void SetLayerMaterial()
    {
        //Get teh material Properties from the clone
        PrefabMaterialProperties materialType = DragHandler.itemBeingDragged.GetComponentInChildren(typeof(PrefabMaterialProperties)) as PrefabMaterialProperties;

        //get the layer ID number to correctly index the list (or array)
        GameObject layerID = this.transform.parent.GetChild(0).gameObject;
        TextMeshProUGUI layerNumber = layerID.GetComponent<TextMeshProUGUI>();
        int i = int.Parse(layerNumber.text);

        //store the material type in the list and tell Unity not to destroy when going to a new scene
        if (materialType != null)  laminateLayers.SetLayerMaterialProperties(i, materialType);

    }

}
