using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public ImpactTesterController controller;
   // public TextMeshProUGUI armAngle;
    public TextMeshProUGUI armHeight;
    // Update is called once per frame
    void Update()
    {
        //armAngle.text = ((controller.PostContactArmAngle)).ToString("F0");
        armHeight.text = (controller.HammerMassHeight).ToString("F2");
    }
}
