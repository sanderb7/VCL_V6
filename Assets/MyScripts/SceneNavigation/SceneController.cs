//this one is used in the isotropoc single layer orthotropic sample scenes
//It initializes the material layers to 1
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private DataController dataController;
    private LaminateLayerMaterials laminateLayerMaterials;
 
        // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        laminateLayerMaterials = FindObjectOfType<LaminateLayerMaterials>();
        dataController.testLaminate.numberOfLayers = 1;
        laminateLayerMaterials.BuildMaterialDataBase();    
    }
  
}
