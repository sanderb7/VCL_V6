//used in the material slot prefab
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{ 
    public GameObject[] labMaterials;
    public GameObject slotPrefab;

    private void Awake()
    {
        int size = labMaterials.Length;
        int month = DateTime.Now.Month;

        //int randomSelection = UnityEngine.Random.Range(0, size - 1);
        //slotPrefab = labMaterials[randomSelection];
        if (size > 1)
            switch (month)
            {
                case 1: //jan
                    slotPrefab = labMaterials[0];
                    break;
                case 2: //feb
                    slotPrefab = labMaterials[0];
                    break;
                case 3: //mar
                    slotPrefab = labMaterials[1];
                    break;
                case 4: //apr
                    slotPrefab = labMaterials[1];
                    break;
                case 5: //May
                    slotPrefab = labMaterials[1];
                    break;
                case 6: //June
                    slotPrefab = labMaterials[1];
                    break;
                case 7: //Jul
                    slotPrefab = labMaterials[2];
                    break;
                case 8: //aug
                    slotPrefab = labMaterials[2];
                    break;
                case 9: //sep
                    slotPrefab = labMaterials[2];
                    break;
                case 10: //oct
                    slotPrefab = labMaterials[3];
                    break;
                case 11: //nov
                    slotPrefab = labMaterials[3];
                    break;
                case 12: //nov
                    slotPrefab = labMaterials[3];
                    break;
                default: //other
                    //System.Random rand = new System.Random();
                    //int index = rand.Next(0, size);
                    //slotPrefab = labMaterials[index];
                    break;
            }
    }
    // Update is called once per frame
    void Update()
    {
        if (this.transform.childCount == 0)
        {
            GameObject slotObject = Instantiate(slotPrefab);
            slotObject.transform.SetParent(this.transform, false);
        }

    }
}
