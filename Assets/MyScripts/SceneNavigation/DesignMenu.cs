//this script controls display of design computations in the orthotropic and multilayer design scences
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DesignMenu : MonoBehaviour
{

    public LaminateMechanics laminateMechanics;
    private DataController dataController;
    private LaminateVisuals laminateVisuals;

    private CompositeProperties compositeProperties;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        laminateVisuals = FindObjectOfType<LaminateVisuals>();

        compositeProperties = FindObjectOfType<CompositeProperties>();
    }

    //display non principle axis properties for use in the single layer, othotropic scene
    public void DisplayMaterialProperties()
    {
        //potential modification - calculate the values are entered and displayed automatically if available
        for (int i = 0; i < dataController.testLaminate.numberOfLayers; i++)
        {
            laminateMechanics.NonPrincipalProperties(i, dataController);

        }
        compositeProperties.DisplayNonPrincipleXYProperties();

    }

    //display structural properties for use in the multilayer, othotropic scene
    public void DisplayStructuralProperties()
    {
        //potential modification - calculate the values are entered and displayed automatically if available
        for (int i = 0; i < dataController.testLaminate.numberOfLayers; i++)
        {
            laminateMechanics.NonPrincipalProperties(i, dataController);
        }

        compositeProperties.DisplayStructuralProperties();
    }
}
