using UnityEngine;
using UnityEngine.UI;

public class ImpactTesterController : MonoBehaviour
{
    [SerializeField]
    private GameObject pivotArm;
    [SerializeField]
    private Rigidbody hammerMass;
    [SerializeField]
    private float armLength;
    [SerializeField]
    private GameObject energyGageDial;

    

    private float postContactArmAngle;
    private float hammerMassHeight;
    private float massStartingHeight;
    private Vector3 originalGageAngle;
    private Vector3 originalHammerMassLocation;

    private Vector3 armOriginalPosition;
    private Rigidbody pivotArmRigidbody;
    [SerializeField]
    private bool specimenImpacted;

    private bool dialSet;
    private bool hasPassedBottom;
    //private float lastEnergyLossMeasure;

    [SerializeField]
    private float absorbedEnergyJ;
    public float AbsorbedEnergyJ { get => absorbedEnergyJ; set => absorbedEnergyJ = value; }
    public float PostContactArmAngle { get => postContactArmAngle; set => postContactArmAngle = value; }
    public float HammerMassHeight { get => hammerMassHeight; set => hammerMassHeight = value; }
    public bool SpecimenImpacted { get => specimenImpacted; set => specimenImpacted = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        armOriginalPosition = pivotArm.transform.eulerAngles;

        hasPassedBottom = false;
        PostContactArmAngle = 0f;
        massStartingHeight = hammerMass.position.y;
        originalGageAngle = energyGageDial.transform.eulerAngles;
        originalHammerMassLocation = hammerMass.transform.position;

        ResetExperiment();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = pivotArm.transform.eulerAngles.z; // change axis if needed
        angle = Mathf.DeltaAngle(0f, angle);

        // Once it crosses bottom and goes positive, start tracking
        if (!hasPassedBottom && angle > 0f)
        {
            hasPassedBottom = true;
        }

        if (hasPassedBottom)
        {
            PostContactArmAngle = Mathf.Max(PostContactArmAngle, angle);
            HammerMassHeight = Mathf.Max(HammerMassHeight, hammerMass.position.y);

            if (pivotArmRigidbody.angularVelocity.magnitude < .05f && !dialSet)
            {
                float deltaHeight = massStartingHeight - HammerMassHeight;
                float energyLoss = -hammerMass.mass * (Physics.gravity.y) * deltaHeight;
                energyGageDial.transform.eulerAngles = originalGageAngle + new Vector3(0, 0, 1) * energyLoss * 10;
                dialSet = true;
            }
        }
        else
        {
            HammerMassHeight = hammerMass.position.y;
        }
    }

    public void ReleaseHammer()
    {
        pivotArmRigidbody.constraints = RigidbodyConstraints.None;
    }
    public void ResetExperiment()
    {
        //reset pivot arm
        pivotArmRigidbody = pivotArm.GetComponent<Rigidbody>();
        pivotArmRigidbody.isKinematic = true;
        hammerMass.isKinematic = true;
        pivotArm.transform.eulerAngles = armOriginalPosition;
        pivotArmRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        pivotArmRigidbody.isKinematic = false;
        hammerMass.isKinematic = false;

        hammerMass.transform.position = originalHammerMassLocation;
        
        //reset experiment measures
        PostContactArmAngle = 0.0f;
        HammerMassHeight = hammerMass.position.y;
        energyGageDial.transform.eulerAngles = originalGageAngle;

        //reset test control booleans
        hasPassedBottom = false;
        SpecimenImpacted = false;
        dialSet = false;
    }
    public void AbsorbImpactEnergy()
    {
        if (SpecimenImpacted) return;

        //Debug.Log(pivotArmRigidbody.angularVelocity + " " + hammerMass.angularVelocity);
        SpecimenImpacted = true;

        Vector3 omega = pivotArmRigidbody.angularVelocity;
        float omegaMag = omega.magnitude;
        if (omegaMag < 0.001f) return;

        float I = hammerMass.mass * armLength * armLength;

        float keBefore = 0.5f * I * omegaMag * omegaMag;
        float keAfter = Mathf.Max(0f, keBefore - absorbedEnergyJ);

        float omegaAfter = Mathf.Sqrt(2f * keAfter / I);

        float deltaOmega = omegaAfter - omegaMag;

        // Convert to angular impulse: ΔL = I Δω
        Vector3 impulse = omega.normalized * (I * deltaOmega);
        pivotArmRigidbody.AddTorque(impulse, ForceMode.Impulse);
        
       // Debug.Log(impulse);

    }
}
