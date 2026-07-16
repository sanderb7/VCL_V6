//this script is used to display the material information 
//it is set up for two didfernt types of material and is dependent upon the Display material boolean as to if they are displayed
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using TexDrawLib;

public class DisplayMaterialInfomation : MonoBehaviour
{
    private GameObject materialInformationPanel;
    private PrefabMaterialProperties material;


    private void Awake()
    {
       materialInformationPanel = GameObject.Find("MaterialInformationPanel"); //need to attach one of these to the material prefab.  this way we can just turn on and off
    }

    public void DisplayMaterialPropertiesOn()
    {

        material = this.GetComponentInChildren(typeof(PrefabMaterialProperties)) as PrefabMaterialProperties;
        if (material != null)
        {
            switch ((int)material.materialType)
            {
                case 1:
                    ShowIsotropicMaterialProperties();
                    break;
                case 2:
                    ShowOrthotropicMaterialProperties();
                    break;
            }
        }
    }

    public void DisplayMaterialPropertiesOff()
    {
        TEXDraw[] textDisplay = materialInformationPanel.GetComponentsInChildren<TEXDraw>();

        for (int i = 0; i < textDisplay.Length; i++)
        {
            textDisplay[i].text = " ";
        }
    }

    private void ShowIsotropicMaterialProperties()
    {
        TEXDraw[] textDisplay = materialInformationPanel.GetComponentsInChildren<TEXDraw>();

        if (material.displayMaterial)
        {
            textDisplay[0].text = material.materialName;
            textDisplay[1].text = "E(Psi)";
            textDisplay[2].text = material.E1.ToString("0.00##e+0");
            textDisplay[3].text = "G_{12}(Psi)";
            textDisplay[4].text = material.G12.ToString("0.00##e+0");
            textDisplay[5].text = "\\nu_{12}";
            textDisplay[6].text = material.nu12.ToString();
            textDisplay[7].text = "\\rho (lb/in^3)";
            textDisplay[8].text = material.density.ToString();
        }
        else
        {
            textDisplay[0].text = material.materialName;
            textDisplay[1].text = "E(Psi)";
            textDisplay[2].text = "???";
            textDisplay[3].text = "G_{12}(Psi)";
            textDisplay[4].text = "???";
            textDisplay[5].text = "\\nu_{12}";
            textDisplay[6].text = "???";
            textDisplay[7].text = "\\rho (lb/in^3)";
            textDisplay[8].text = "???";
        }
    }

    private void ShowOrthotropicMaterialProperties()
    {
        TEXDraw[] textDisplay = materialInformationPanel.GetComponentsInChildren<TEXDraw>();

        if (material.displayMaterial)
        {
            textDisplay[0].text = material.materialName;
            textDisplay[1].text = "E_1(Psi)";
            textDisplay[2].text = material.E1.ToString("0.00##e+0");
            textDisplay[3].text = "E_2(Psi)";
            textDisplay[4].text = material.E2.ToString("0.00##e+0");
            textDisplay[5].text = "G_{12}(Psi)";
            textDisplay[6].text = material.G12.ToString("0.00##e+0");
            textDisplay[7].text = "\\nu_{12}";
            textDisplay[8].text = material.nu12.ToString();
            textDisplay[9].text = "\\rho (lb/in^3)";
            textDisplay[10].text = material.density.ToString();
        }
        else
        {
            textDisplay[0].text = material.materialName;
            textDisplay[1].text = "E_1(Psi)";
            textDisplay[2].text = "???";
            textDisplay[3].text = "E_2(Psi)";
            textDisplay[4].text = "???";
            textDisplay[5].text = "G_{12}(Psi)";
            textDisplay[6].text = "???";
            textDisplay[7].text = "\\nu_{12}";
            textDisplay[8].text = "???";
            textDisplay[9].text = "\\rho (lb/in^3)";
            textDisplay[10].text = "???";
        }
    }
}
