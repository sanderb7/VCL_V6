using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompositeProperties : MonoBehaviour
{
    public TextMeshProUGUI compositeBendingStiffnessDisplay;
    public TextMeshProUGUI compositeIxxDisplay;
    public TextMeshProUGUI compositeWeightDisplay;

    public TextMeshProUGUI Ex;
    public TextMeshProUGUI Ey;
    public TextMeshProUGUI Gxy;
    public TextMeshProUGUI nuxy;

    private DataController dataController;
    public LaminateMechanics laminate;

    //Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
    }
    //private void Update()
    //{
    //    this would be a good place to update the material properties continouously if desired
    //    would need make it dependent on the scene.  could do this via a switch statement as outlined below

    //    switch (case)
    //    {
    //    case 1: DisplayNonPrincipleXYProperties(); break;
    //    case 2: DisplayStructuralProperties(); break;
    //    default: break;
    //    }
    //}

    public void DisplayNonPrincipleXYProperties()
    {
        bool display = dataController.testLaminate.lamina[0].materialProperties.displayProperties;
        if (display)
        {
            Ex.text = dataController.testLaminate.lamina[0].materialProperties.Ex.ToString("e2");
            Ey.text = dataController.testLaminate.lamina[0].materialProperties.Ey.ToString("e2");
            Gxy.text = dataController.testLaminate.lamina[0].materialProperties.Gxy.ToString("e2");
            nuxy.text = dataController.testLaminate.lamina[0].materialProperties.nuxy.ToString("f2");
        }
        else
        {
            Ex.text   = "???";
            Ey.text   = "???";
            Gxy.text  = "???";
            nuxy.text = "???";
        }
    }

    public void DisplayStructuralProperties()
    {

        float EI               = laminate.EquivalentBendingStiffness(dataController);
        float totalIxx         = laminate.SecondMomentOfInertia(dataController);
        float laminateWeight   = laminate.LaminateWeight(dataController);

     
        compositeBendingStiffnessDisplay.text = EI.ToString("e2");
        compositeIxxDisplay.text              = totalIxx.ToString("e2");
        compositeWeightDisplay.text           = laminateWeight.ToString();

        //some stuff I changed - keeping here in case I want to bring back in but at this 
        //point it does not suit the learning objective
        //if (totalIxx !=0.0f)
        //  effectiveModulus = EI / totalIxx;
        //compositeModulusDisplay.text          = effectiveModulus.ToString("e2");

    }
}
