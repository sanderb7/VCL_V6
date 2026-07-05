using UnityEngine;

public class MaterialSelection : MonoBehaviour
{
    [SerializeField]
    private float[] material;
    [SerializeField]
    ImpactTesterController controller;

    private void Start()
    {
        controller.AbsorbedEnergyJ = material[0];
    }
    public void MaterialSelecton (int selection)
    {
        controller.AbsorbedEnergyJ = material[selection];
    }
    
}
