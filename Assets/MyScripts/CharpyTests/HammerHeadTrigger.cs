using UnityEngine;

public class HammerHeadTrigger : MonoBehaviour
{
    public ImpactTesterController impactTesterController;
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        impactTesterController.AbsorbImpactEnergy();
    }
    public void ResetCollider()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
}
