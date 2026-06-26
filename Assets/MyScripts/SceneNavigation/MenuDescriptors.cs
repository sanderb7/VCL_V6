//////
///This script is used in menus to provide a discription of where the button selection takes the user

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDescriptors : MonoBehaviour
{

    public TEXDraw textDrawDisplay;


    // Start is called before the first frame update

    public void InterfaceTrainingButtonText()
    {
//        textDisplay.text = "Learn How to Use the Simulation Features";
    }

    public void AssesmentButtonText()
    {
//        textDisplay.text = "Test Your Knowledge";
    }

    public void NonDirectionalMaterialDesign()
    {
        textDrawDisplay.text = "\\begin{flushleft} Design a isotropic material sample.  Two independent material properties required.\n\n\\end{flushleft}  \\begin{center}G_{12}=\\frac{E}{2(1 +\\nu_{12})}\\end{center}";

    }

    public void SingleLayerOrthrotropicMaterialDesign()
    {
        textDrawDisplay.text = "\\begin{flushleft}Design a single layer, directionally dependent material for the case of plane stress.  This is classified as an {\\bf Orthotropic} material.  Four independent material properties required:\\end{flushleft}" +
            " \\begin{center}E_1, E_2, G_{12}, \\nu_{12}\\end{center}";
    }

    public void MultiLayerOrthrotropicMaterialDesign()
    {
        textDrawDisplay.text = "\\begin{flushleft}Design a multilayer, orthotropic material.\n\n\\end{flushleft}";
    }


    public void ClearMessageText()
    {
        textDrawDisplay.text = " ";
    }

    //keep this around to jar my memory on a way to use this in multiple locations
    public void OldRoutine()
    {
        //switch (dataController.experimentType)
        //{
        //    case 1:
        //        textDisplay.text = "Observe and discuss the behavior of a particle moving in a magnetic field ";
        //        break;
        //    case 2:
        //        textDisplay.text = "Observe and discuss the behavior of a current carrying wire in a magnetic field";
        //        break;
        //    case 3:
        //        textDisplay.text = "Observe and discuss the behavior of a torque on a current loop";
        //        break;
        //    default:
        //        break;
        //}
    }
}
