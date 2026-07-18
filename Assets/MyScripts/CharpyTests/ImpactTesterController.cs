using UnityEngine;
using System.Collections;
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
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip audioClip;

    //
    private float postContactArmAngle;
    private float hammerMassHeight;
    private float massStartingHeight;
    private Quaternion originalGageRotation;
    private float startingZAngle;
    private Vector3 originalHammerMassLocation;

    private Vector3 armOriginalPosition;
    private Rigidbody pivotArmRigidbody;
    [SerializeField]
    private bool specimenImpacted;

    private bool dialSet;
    private bool hasPassedBottom;
    private float energyLossMeasure;

    //Pedulum Control
    private bool returning;
    private Quaternion bottomRotation;

    [SerializeField]
    private float absorbedEnergyJ;
    public float AbsorbedEnergyJ { get => absorbedEnergyJ; set => absorbedEnergyJ = value; }
    public float PostContactArmAngle { get => postContactArmAngle; set => postContactArmAngle = value; }
    public float HammerMassHeight { get => hammerMassHeight; set => hammerMassHeight = value; }
    public bool SpecimenImpacted { get => specimenImpacted; set => specimenImpacted = value; }
    public float EnergyLossMeasure { get => energyLossMeasure; set => energyLossMeasure = value; }

    //varible to account for variation on material variation
    private float materialSampleEnergyLoss;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pivotArmRigidbody = pivotArm.GetComponent<Rigidbody>();
        //armOriginalPosition = pivotArm.transform.eulerAngles;
        bottomRotation = Quaternion.Euler(90f, 0f, 0f); ;

        hasPassedBottom = false;
        PostContactArmAngle = 0f;
       // massStartingHeight = hammerMass.position.y;
        originalGageRotation = energyGageDial.transform.localRotation;
        //originalHammerMassLocation = hammerMass.transform.position;
        originalHammerMassLocation = hammerMass.transform.localPosition;

        // ResetExperiment();
        returning = false;
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
                //float deltaHeight = massStartingHeight - HammerMassHeight;
                //float energyLoss = -hammerMass.mass * (Physics.gravity.y) * deltaHeight;

                //float normalizedEnergy = energyLoss / 10.0f;
                float normalizedEnergy = materialSampleEnergyLoss / 10.0f;
                float rotationOffset = 154.0f * normalizedEnergy; //117 is the angle range of the dial
                energyGageDial.transform.localRotation = originalGageRotation*Quaternion.Euler(0, 0, rotationOffset);
                dialSet = true;

                // energyLossMeasure = absorbedEnergyJ * 30.0f;
                energyLossMeasure = materialSampleEnergyLoss * 30.0f;

                StartCoroutine(StopPendulum());
            }
        }
        else
        {
            HammerMassHeight = hammerMass.position.y;
        }
    }

    public void ReleaseHammer()
    {
        materialSampleEnergyLoss = absorbedEnergyJ * Random.Range(0.9f, 1.1f);
        pivotArmRigidbody.constraints = RigidbodyConstraints.None;
    }
    public void ResetExperiment()
    {
        //reset pivot arm        
        StartCoroutine(ResetPendulum());
        pivotArmRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        //reset experiment measures        
        PostContactArmAngle = 0.0f;
        energyGageDial.transform.localRotation = originalGageRotation;
        energyLossMeasure = 0.0f;

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
//        float keAfter = Mathf.Max(0f, keBefore - absorbedEnergyJ);
        float keAfter = Mathf.Max(0f, keBefore - materialSampleEnergyLoss);

        float omegaAfter = Mathf.Sqrt(2f * keAfter / I);

        float deltaOmega = omegaAfter - omegaMag;

        // Convert to angular impulse: ΔL = I Δω
        Vector3 impulse = omega.normalized * (I * deltaOmega);
        pivotArmRigidbody.AddTorque(impulse, ForceMode.Impulse);
        
       // Debug.Log(impulse);

    }
    IEnumerator StopPendulum()
    {
        if (returning) yield break;

        returning = true;

        // Stop the physics
        pivotArmRigidbody.isKinematic = true;
        hammerMass.isKinematic = true;

        Quaternion startRotation = pivotArm.transform.localRotation;
        Quaternion endRotation = bottomRotation;

        float t = 0f;
        float duration = 1.5f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            pivotArm.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        pivotArm.transform.localRotation = endRotation;
        returning = false;
    }
    IEnumerator ResetPendulum()
    {
        if (returning) yield break;

        returning = true;

        // Stop the physics
        pivotArmRigidbody.isKinematic = true;
        hammerMass.isKinematic = true;

        Quaternion startRotation = pivotArm.transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(90, 0, -129);

        float t = 0f;
        float duration = 1.5f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            pivotArm.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        pivotArm.transform.localRotation = endRotation;
        audioSource.PlayOneShot(audioClip);

        //        hammerMass.transform.position = originalHammerMassLocation;
        hammerMass.transform.localPosition = originalHammerMassLocation;

        pivotArmRigidbody.isKinematic = false;
        hammerMass.isKinematic = false;
        returning = false;
    }
}
