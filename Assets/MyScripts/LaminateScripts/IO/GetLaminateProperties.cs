//get laminate properties and store
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetLaminateProperties : MonoBehaviour
{
    private DataController dataController;

    private void Start()
    {
        dataController = FindObjectOfType<DataController>();

        if (dataController.testLaminate.length != 0.0f)  DisplayProperties();
    }
    
    public void DisplayProperties()
    {
        Text[] laminatePropertiesDisplay = this.transform.GetComponentsInChildren<Text>();

        laminatePropertiesDisplay[0].text = dataController.testLaminate.Name;
        laminatePropertiesDisplay[2].text = dataController.testLaminate.length.ToString();
        laminatePropertiesDisplay[4].text = dataController.testLaminate.width.ToString();
        laminatePropertiesDisplay[6].text = dataController.testLaminate.thickness.ToString();
        laminatePropertiesDisplay[8].text = dataController.testLaminate.numberOfLayers.ToString();
    }
    public void GetLaminateName(string newText)
    {
        string laminateName = newText;
         dataController.testLaminate.Name = laminateName;
        //        Debug.Log("new text = " + laminateName);
    }
    public void GetLaminateLength(string newText)
    {
        float length = float.Parse(newText);
        dataController.testLaminate.length = length;
        //        Debug.Log("Laminate length = " +  length);
    }
    public void GetLaminateWidth(string newText)
    {
        float width = float.Parse(newText);
        dataController.testLaminate.width = width;
        //        Debug.Log("Laminate Width = " + width);
    }
    public void GetLaminateThickness(string newText)
    {
        float thickness = float.Parse(newText);
        dataController.testLaminate.thickness = thickness;
        //        Debug.Log("Laminate Thickness =  " + thickness);
    }
    public void GetNumberofLayers(string newText)
    {
        int numberOfLayers = int.Parse(newText);
        dataController.testLaminate.numberOfLayers = numberOfLayers;
        //        Debug.Log("Number of Layers =  " + numberOfLayers);
    }
}
