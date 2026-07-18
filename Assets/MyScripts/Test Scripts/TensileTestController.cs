//this script controls the behavior of a tensile test.
// (1) in the start function the initial mechanical properties are computed depending on the test type and the output file is opened
// (2) A tensile test can then be controlled via a slided.  Note that the detailed functions are dependent up material type (i.e., directional vs non directional)
//

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GraphMaster;

public class TensileTestController : MonoBehaviour
{
    public GameObject loadApplicatorTop;
    public GameObject loadApplicatorBottom;

    public Transform laminatePositionInLoadCell;
    public Slider mainSlider;
    public AudioSource audiosource;
    public AudioClip audioClip;
    public LaminateVisuals laminateVisuals;

    public GameObject dogBoneTestSpecimen;
    public GameObject laminatePrefab;

    public ScatterGraphScript graph;

    public TextMeshProUGUI loadDisplay;
    public TextMeshProUGUI displacementDisplay;

    public static float appliedLoad;
    public static float scaleFactor;
    public static float maxDisplacement;
    //    public static bool loadChange = false;
    private bool specimenFailure = false;

    private float yieldStress; //psi
    private float ultimateTensileStress; //psi
    private float failureStrain;
    private float yieldStrain;

    ///////////////// This is something that is subject to be a user defined variable in the future //////////////////////////
    private float gaugeLength           = 2.0f;          //inches
    private float gaugeLengthTransverse = 0.5f; //inches
    private float crossSectionArea      = 0.5f*0.25f; //sq inches

    //create an instance of matrix operations
    private MatrixOperations matrixOperations = new MatrixOperations();

    [HideInInspector]
    public LaminateMechanics laminate;
    private DataController dataController;

    private TwistingMeshDeformer twistingMeshDeformer;


    private float[,] complianceMatrix = new float[3, 3];
    private float[,] stiffnessMatrix = new float[3, 3];
    private float[,] ABDMatrix = new float[6, 6];
    private float[,] inverseABDMatrix = new float[6, 6];

    public static bool loadChange = false;
    public static float curvature;

    private string testOutputfileName;

    private void Start()
    {

        //find the game objects that have the laminate data (DataController) and the visualization method
        dataController = FindAnyObjectByType<DataController>();
        if (dataController.testType == TestType.threePointBendTensileTest || dataController.testType == TestType.charpyTest) return;
 

        OpenOutputFile(dataController.testType);
        SetModelParameters();  //

        
        //intiate the listener to check for a load change
        mainSlider.onValueChanged.AddListener(delegate { LoadChangeCheck(); });

        if(dataController.testType == TestType.orthotropicMaterialTensileTest)
        {
            laminate.NonPrincipalProperties(0, dataController);
            complianceMatrix = laminate.TransformedComplianceMatrix(0, dataController);
        }

        if (dataController.testType == TestType.multilayerMaterialTensileTest)
        {
            dogBoneTestSpecimen.SetActive(false);

            ABDMatrix =  laminate.FindABDMatrix(dataController);
            inverseABDMatrix = matrixOperations.MatrixInverse6x6(ABDMatrix);
            laminateVisuals = FindAnyObjectByType<LaminateVisuals>();
            laminateVisuals.BuildTensileTestLaminate(laminatePrefab, laminatePositionInLoadCell);
        }      
    }


    private void SetModelParameters()
    {
        yieldStress = dataController.testLaminate.lamina[0].materialProperties.yieldStress; //psi
        ultimateTensileStress = dataController.testLaminate.lamina[0].materialProperties.ultimateTensileStress; ; //psi
        failureStrain = dataController.testLaminate.lamina[0].materialProperties.failureStrain; ;
        yieldStrain = dataController.testLaminate.lamina[0].materialProperties.yieldStrain; ;
    }

    // Invoked when the value of the slider changes.
    public void LoadChangeCheck()
    {
        switch ((int)dataController.testType)
        {
            case (int)TestType.isotropicMaterialTensileTest:
                if (!specimenFailure) TensileTestElasticPlasticMaterial();
                break;
            case (int)TestType.orthotropicMaterialTensileTest:
                TensileTestLinearElasticMaterial(); break;
            case (int)TestType.multilayerMaterialTensileTest:
                loadChange = true;
                TensileTestMultiLayerElasticMaterial();
                break;
            default: break;
        }
    }

    // for use with Classical lamination theory and finding strain based on a running load Nx
    public void TensileTestMultiLayerElasticMaterial()
    {
        float[] runningLoadVector = {mainSlider.value / dataController.testLaminate.width, 0, 0, 0, 0, 0};
        float[] strainVector = new float[6];
        strainVector = matrixOperations.MatrixVectorMultiplication6x6(inverseABDMatrix, runningLoadVector, 6);

        //Output results

        loadDisplay.text = ((int)mainSlider.value).ToString();
        displacementDisplay.text = "see file";

        RecordData(runningLoadVector[0], strainVector);
        graph.UpdateGraphMultiLayerOrthotropic(runningLoadVector[0], strainVector[0], strainVector[5]);

        //this is for visualization purposes - it exaggerates the twisting motion
        curvature = 1000.0f*strainVector[5];
        loadApplicatorTop.transform.localEulerAngles    = new Vector3(0,  curvature/10.0f, 0);
        loadApplicatorBottom.transform.localEulerAngles = new Vector3(0, -curvature/10.0f, 180);
    }
    //for use when simulating a single layer orthotropic material strain = complianceMatrix*stress
    public void TensileTestLinearElasticMaterial()
    {
        float[] appliedStressVector = { mainSlider.value / crossSectionArea, 0, 0 };
        float[] strainVector = matrixOperations.MatrixVectorMultiplication3x3(complianceMatrix, appliedStressVector, 3);

        float displacementX = strainVector[0] * gaugeLength;
        float displacementY = strainVector[1] * gaugeLengthTransverse;

        //Output results
        loadDisplay.text = ((int)mainSlider.value).ToString();
        displacementDisplay.text = displacementX.ToString("f4");

        RecordData(appliedStressVector[0], strainVector[0], strainVector[1]);
        graph.UpdateGraphOrthotropic(appliedStressVector[0], strainVector[0], strainVector[1]);
    }

    //this is for use with an elastic plastic material - it uses the Ramberg-Osgood relationship to simulate the material response
    public void TensileTestElasticPlasticMaterial()
    {

        //n is a model parameters
        float n = Mathf.Log10(ultimateTensileStress / yieldStress) / Mathf.Log10(failureStrain / yieldStrain);
    
        float appliedStress = mainSlider.value / crossSectionArea;
        float strain = appliedStress / dataController.testLaminate.lamina[0].materialProperties.Ex + .002f * Mathf.Pow(appliedStress / yieldStress, 1.0f / n);
        float displacement = strain * gaugeLength;

        //output results
        loadDisplay.text = ((int)mainSlider.value).ToString();
        displacementDisplay.text = displacement.ToString("f4");

        RecordData(mainSlider.value, displacement);
        graph.UpdateGraphIsotropic(displacement, mainSlider.value);

        if(strain > failureStrain)
        {
            audiosource.PlayOneShot(audioClip);
            specimenFailure = true;
        }
    }
    //Record data - 3 overloads depending on the test type

    //use this creating one x-y output
    void RecordData(float appliedLoad, float displacement)
    {
        string filePath = Path.Combine(dataController.testDataOutputPath, testOutputfileName);

        using (StreamWriter file = new StreamWriter(filePath, true))
        {
            file.WriteLine("{0} ,  {1}", appliedLoad, displacement);
        }
    }

    //use this one for 2D analysis - creating two x-y outputs
    void RecordData(float x, float y1, float y2)
    {
        string filePath = Path.Combine(dataController.testDataOutputPath, testOutputfileName);

        using (StreamWriter file = new StreamWriter(filePath, true))
        {
            file.WriteLine("{0} ,  {1},   {2}", x, y1, y2);
        }
    }

   //use this one when obtaining values from the CLT analysis and has outputs Nx in addition to the entire strain vector
    void RecordData(float x, float [] vector)
    {
        //        string filePath = Path.Combine(Application.streamingAssetsPath, testOutputfileName);
        string filePath = Path.Combine(dataController.testDataOutputPath, testOutputfileName);

        using (StreamWriter file = new StreamWriter(filePath, true))
        {
            file.WriteLine("{0} , {1:E3}, {2:E3}, {3:E3}, {4:E3},  {5:E3}, {6:E3}", x, vector[0], vector[1], vector[2], vector[3], vector[4], vector[5]);
        }
    }


    //open the output file and write the appropriate header information
    private void OpenOutputFile(TestType testType)
    {
        testOutputfileName = dataController.testLaminate.Name + ".csv";
        string filePath = Path.Combine(dataController.testDataOutputPath, testOutputfileName);

        using (StreamWriter file = new StreamWriter(filePath, false))
        {
            file.WriteLine("Sample Name,  {0}", dataController.testLaminate.Name);
            switch ((int)testType)
            {
                case (int)TestType.isotropicMaterialTensileTest:
                    file.WriteLine("Cross Section Area (sq in), {0}", crossSectionArea);
                    file.WriteLine("Gauge Length (in), {0}", gaugeLength);
                    file.WriteLine("Applied Load (lbs), Displacement (in)");
                    break;
                case (int)TestType.orthotropicMaterialTensileTest:
                    file.WriteLine("Cross Section Area (sq in), {0}", crossSectionArea);
                    file.WriteLine("Gauge Length (in), {0}", gaugeLength);
                    file.WriteLine("stress (lbs/sq in), Strain x (in/in), strain y (in/in)");
                    break;
                case (int)TestType.multilayerMaterialTensileTest:
                    file.WriteLine("Laminate Length(in),     {0}", dataController.testLaminate.length);
                    file.WriteLine("Lamiante Width (in),     {0}", dataController.testLaminate.width);
                    file.WriteLine("Lamiante Thickness (in), {0}", dataController.testLaminate.thickness);
                    file.WriteLine("Bending Stiffness (Psi-in ^4), {0:E3}", laminate.EquivalentBendingStiffness(dataController));
                    file.WriteLine("Ixx (in ^4), {0:E3}", laminate.SecondMomentOfInertia(dataController));
                    file.WriteLine("Weight (lbs), {0:E3}", laminate.LaminateWeight(dataController));
                    file.WriteLine("Nx (lbs/in), Strain x (in/in), strain y (in/in),  strain  xy (in/in), kappa x, kappa y, kappa xy");
                    break;
            }
        }
    }
    public void CloseFile()
    {
        string filePath = Path.Combine(dataController.testDataOutputPath, testOutputfileName);

        using (StreamWriter file = new StreamWriter(filePath, true))
        {
            file.Close();
        }

    }

    //these were for verifying some calculations - will keep for future use
    private void Display6x6Matrix(float[,] matrix)
    {
        for (int i = 0; i < 6; i++)
        {
            Debug.LogFormat("{0:E2}  {1:E2}  {2:E2}   {3:E2}  {4:E2}  {5:E2}", matrix[i, 0], matrix[i, 1], matrix[i, 2], matrix[i, 3], matrix[i, 4], matrix[i, 5]);
        }

        Debug.Log(" ");
    }
    private void Display3x3Matrix(float[,] matrix)
    {
        int n = 3;
        for (int k = 0; k < n; k++)
        {
            Debug.LogFormat("{0:E3}  {1:E3}  {2:E3}", matrix[k, 0], matrix[k, 1], matrix[k, 2]);
        }

        Debug.Log(" ");
    }

}
