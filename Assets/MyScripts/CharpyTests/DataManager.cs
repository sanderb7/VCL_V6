using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public ImpactTesterController controller;
    public TextMeshProUGUI energyDissipated;
    // Update is called once per frame
    void Update()
    {

        energyDissipated.text = (controller.EnergyLossMeasure).ToString("F0");
    }
}
