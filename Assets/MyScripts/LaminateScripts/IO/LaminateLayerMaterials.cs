// this is used for getting laminate layer properties.  It creates amd displays a row of requested information based on a prefab./
// the user than populations in the input fields

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LaminateLayerMaterials : MonoBehaviour
{
    public Transform materialLayerParent;
    public GameObject materialLayerPreFab;
    
    public void BuildMaterialDataBase()
    { 
        DataController dataController = FindObjectOfType<DataController>();

        //clear existing material list if it exists
        if (dataController.testLaminate.lamina.Count > 0) dataController.testLaminate.lamina.Clear();
        foreach  (Transform child in materialLayerParent.transform)
        {
            Destroy(child.gameObject);
        }

        //instantiate a blank layer prefab for each laminate layer
        for (int i = 0; i < dataController.testLaminate.numberOfLayers; i++)
        {
            GameObject materialLayerInstance = (GameObject)Instantiate(materialLayerPreFab);
            materialLayerInstance.transform.SetParent(materialLayerParent);

            materialLayerInstance.transform.localScale = Vector3.one;
            materialLayerInstance.transform.localPosition = new Vector3(materialLayerParent.position.x, materialLayerParent.position.y, 0.0f);
            TextMeshProUGUI[] layerNames = materialLayerInstance.GetComponentsInChildren<TextMeshProUGUI>();

            InputField[] inputs = materialLayerInstance.GetComponentsInChildren<InputField>();
            layerNames[0].text = (i + 1).ToString();
            materialLayerInstance.name = "layer" + (i + 1);
            dataController.testLaminate.lamina.Add(new Lamina());
        }
    }
}
