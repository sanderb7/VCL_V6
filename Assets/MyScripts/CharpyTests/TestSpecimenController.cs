using UnityEngine;

public class TestSpecimenController : MonoBehaviour
{
    public ImpactTesterController impactTesterController;
    public GameObject[] testSpecimen;
    public GameObject dummySpecimen;

    Rigidbody[] rb = new Rigidbody[2];
    MeshRenderer[] testSpecimenRenderer = new MeshRenderer[2];
    MeshRenderer dummySpecimenRenderer;
    Vector3[] testSpecimenOriginalPosition = new Vector3[2];

    void Start()
    {
        for (int i = 0; i < testSpecimen.Length; i++)
        {
            testSpecimenOriginalPosition[i] = testSpecimen[i].transform.localPosition;
            rb[i] = testSpecimen[i].GetComponent<Rigidbody>();
            testSpecimenRenderer[i] = testSpecimen[i].GetComponent<MeshRenderer>();
            dummySpecimenRenderer = dummySpecimen.GetComponent<MeshRenderer>();

            testSpecimenRenderer[i].enabled = false;
            rb[i].useGravity = false;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        ApplyForce();
        impactTesterController.AbsorbImpactEnergy();
        dummySpecimenRenderer.enabled = false;
    }
   
    private void ApplyForce()
    {
        if (impactTesterController.SpecimenImpacted) return;
        
        for (int i = 0; i < testSpecimen.Length; i++)
        {
            rb[i].AddRelativeForce(new Vector3(1.0f, 0, 0)/impactTesterController.AbsorbedEnergyJ, ForceMode.Impulse);
            rb[i].useGravity = true;
            testSpecimenRenderer[i].enabled = true;
           // Debug.Log("force applied");
        }
    }
    public void ResetTestSpecimen()
    {
        for (int i = 0; i < testSpecimen.Length; i++)
        {
            testSpecimenRenderer[i].enabled = false;
            rb[i].isKinematic = true;
            testSpecimen[i].transform.localPosition = testSpecimenOriginalPosition[i];
            testSpecimen[i].transform.localRotation = Quaternion.identity;
            rb[i].useGravity = false;
            rb[i].isKinematic = false;

            dummySpecimenRenderer.enabled = true;
        }
    }
}
