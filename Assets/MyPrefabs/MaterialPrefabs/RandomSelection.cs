using System;
using UnityEngine;

public class RandomSelection : MonoBehaviour
{
    public GameObject[] Materials;
  
    private void Awake()
    {
        //var slot = GetComponent<SlotController>().slotPrefab;
        //int month = DateTime.Now.Month;

        //int randomSelection = UnityEngine.Random.Range(0, 4);
        //slot = unknownMaterials[randomSelection];
        //Debug.Log(randomSelection + " " + slot);

        //switch (month)
        //{
        //    case 1: //jan
        //        _ = unknownMaterials[0];
        //        break;
        //    case 3: //mar
        //        _ = unknownMaterials[1];
        //        break;
        //    case 8: //aug
        //        Debug.Log(month);
        //        _ = unknownMaterials[2];
        //        break;
        //    case 10: //oct
        //        _ = unknownMaterials[3];
        //        break;

        //    default: //other
        //        System.Random rand = new System.Random();
        //        int index = rand.Next(0,4);
        //        _ = unknownMaterials[index];
        //        break;
        //}
    }
}
