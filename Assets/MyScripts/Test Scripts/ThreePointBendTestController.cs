//simulates a three point bend test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThreePointBendTestController : MonoBehaviour
{
    //3 pointBendTestController
    public GameObject loadApplicator;
    public Transform loadApplicatorTip;
    public GameObject laminatePrefab;
    public Slider mainSlider;
    public LaminateVisuals laminateVisuals;


    public TextMeshProUGUI loadDisplay;
    public TextMeshProUGUI displacementDisplay;

    public static float appliedLoad;
    public static float scaleFactor;
    public static float maxDisplacement;
    public static bool loadChange = false;
    private bool specimenFailure = false;
    public AudioSource audioSource;


    public LaminateMechanics laminate;
    private DataController dataController;

    private float inchesPerMeters = 39.37f;  //[39.37 inches/meter] - conversion 

    private Vector3 initialLoadApplicatorPosition = new Vector3();


    private void Start()
    {

        //find the game objects that have the laminate data (DataController) and the visualization method
        dataController = FindObjectOfType<DataController>();
        laminateVisuals = FindObjectOfType<LaminateVisuals>();

        //intiate the listener to check for a load change
        mainSlider.onValueChanged.AddListener(delegate { LoadChangeCheck(); });

        //compute the material properties based on the layers orientation
        for (int i = 0; i < dataController.testLaminate.numberOfLayers; i++)
            laminate.NonPrincipalProperties(i, dataController);

        laminateVisuals.BuildThreePointBendTestLaminate(laminatePrefab, loadApplicatorTip);
        initialLoadApplicatorPosition = loadApplicator.transform.localPosition;

    }

    // Invoked when the value of the slider changes.
    public void LoadChangeCheck()
    {
        loadChange = true; //this is used for to control the mesh deformation methods
        appliedLoad = mainSlider.value;
        if(!specimenFailure) ThreePointBendTest();
    }

    public void ThreePointBendTest()
    {
        //can make these part of a setup method
        float EI = laminate.EquivalentBendingStiffness(dataController);
        scaleFactor = -appliedLoad * Mathf.Pow(dataController.testLaminate.length, 2) / (48.0f * EI);
        float maxDisplacement = (scaleFactor * dataController.testLaminate.length);

        //adjust the position of the load applicator based on the deflection of the beam
        loadApplicator.transform.localPosition = new Vector3(initialLoadApplicatorPosition.x, initialLoadApplicatorPosition.y + maxDisplacement / inchesPerMeters, initialLoadApplicatorPosition.z);

        for (int x = 0; x <= dataController.testLaminate.length / 2; x++)
        {
            //will put a stress calculation here
            //insert failure criteria
        }

        //display current experiment conditions
        loadDisplay.text = ((int) appliedLoad).ToString();
        displacementDisplay.text = (-maxDisplacement).ToString("F4");

        if (Mathf.Abs(maxDisplacement) >= 1.0f)
        {
            audioSource.Play();
            specimenFailure = true;
        }

    }
}
