//given a laminate with multiple layers this stores those values for each individual layea.  Values include:
// - layer thickness
// - layer orientation
// - layer material properties

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetLayerProperties : MonoBehaviour
{

    public float layerOrientation;

    private DataController dataController;

    //Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
    }

    public void GetLayerThickness(string newText)
    {

        float layerThickness = float.Parse(newText);

        GameObject layerID = this.transform.GetChild(0).gameObject;
        TextMeshProUGUI layerNumber = layerID.GetComponent<TextMeshProUGUI>();
        int i = int.Parse(layerNumber.text);

        try
        {
            dataController.testLaminate.lamina[i - 1].physicalProperties.thickness = layerThickness;
        }
        catch
        {
            Debug.Log("layer thickness not set");
        }

    }
    public void GetLayerOrientation(string newText)
    {
        //float layerOrientation = float.Parse(newText);
        layerOrientation = float.Parse(newText);

        GameObject layerID = this.transform.GetChild(0).gameObject;
        TextMeshProUGUI layerNumber = layerID.GetComponent<TextMeshProUGUI>();
        int i = int.Parse(layerNumber.text);
        dataController.testLaminate.lamina[i - 1].physicalProperties.orientation = layerOrientation;
    }

    public void SetLayerMaterialProperties(int i, PrefabMaterialProperties materialType)
    {
        dataController.testLaminate.lamina[i - 1].materialProperties.materialName = materialType.materialName;
        dataController.testLaminate.lamina[i - 1].materialProperties.E1 = materialType.E1;
        dataController.testLaminate.lamina[i - 1].materialProperties.E2 = materialType.E2;
        dataController.testLaminate.lamina[i - 1].materialProperties.G12 = materialType.G12;
        dataController.testLaminate.lamina[i - 1].materialProperties.nu12 = materialType.nu12;
        dataController.testLaminate.lamina[i - 1].materialProperties.density = materialType.density;
        dataController.testLaminate.lamina[i - 1].materialProperties.material = materialType.material;

        dataController.testLaminate.lamina[i - 1].materialProperties.yieldStress = materialType.yieldStress;
        dataController.testLaminate.lamina[i - 1].materialProperties.ultimateTensileStress = materialType.ultimateTensileStress;
        dataController.testLaminate.lamina[i - 1].materialProperties.failureStrain = materialType.failureStrain;
        dataController.testLaminate.lamina[i - 1].materialProperties.yieldStrain = materialType.yieldStrain;

        dataController.testLaminate.lamina[i - 1].materialProperties.displayProperties = materialType.displayMaterial;

    }

}
