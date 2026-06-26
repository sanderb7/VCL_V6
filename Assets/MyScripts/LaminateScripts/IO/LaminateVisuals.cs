//the class builds laminate representations for rectangular samples for the 3 point bendtest and tensile test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaminateVisuals : MonoBehaviour
{
    //public GameObject matrixLayerPrefab;
    //public GameObject fiberPrefab;

    public GameObject supportPrefab;
    public GameObject upperGripperConf;
    public GameObject lowerGripperConf;

    private readonly float inchesPerMeter = 39.37f;  //inches per meter
    private DataController dataController;

    // Start is called before the first frame update
    void Start()
    {
       dataController = FindObjectOfType<DataController>();
    }

    public void BuildThreePointBendTestLaminate(GameObject laminatePrefab, Transform parent)
    {
        dataController = FindObjectOfType<DataController>();

        //build the visual representation

        //measurements in x, y and z or (length, thickness, and width
        float laminateLength    = dataController.testLaminate.length;
        float laminateThickness = dataController.testLaminate.thickness;
        float laminateWidth     = dataController.testLaminate.width;

        // note that the prefab mesh is a .3m cube but the dimensions units are given in inches.  Unity needs this in meters, so created a
        //coversion to first convert the mesh to 1 cubic m (1/.3) and then scale it by the inches per meter factor to get meters
        GameObject laminateInstance = (GameObject)Instantiate(laminatePrefab);
        laminateInstance.transform.SetParent(parent.transform);
        laminateInstance.transform.localPosition = new Vector3(0, -laminateThickness / inchesPerMeter / 2.0f, 0);
        laminateInstance.transform.localEulerAngles = Vector3.zero;
        laminateInstance.transform.localScale = new Vector3(laminateLength, laminateThickness, laminateWidth) * ((1.0f / 0.3f) / inchesPerMeter); //[in][m/m][m/in]
        laminateInstance.name = "TestSample";

        //setup supports
        //note that the -.0473 is the center of the support arrow prefab - probably better off putting this in a empty go or moving it to the center in Maya
        GameObject leftSupportInstance = (GameObject)Instantiate(supportPrefab);
        leftSupportInstance.transform.SetParent(parent.transform);
        leftSupportInstance.transform.localPosition = new Vector3(0, -.0473f, 0) + new Vector3(-laminateLength / 2.0f / inchesPerMeter, -laminateThickness/ inchesPerMeter, 0.0f);

        GameObject rightSupportInstance = (GameObject)Instantiate(supportPrefab);
        rightSupportInstance.transform.SetParent(parent.transform);
        rightSupportInstance.transform.localPosition = new Vector3(0, -.0473f, 0) + new Vector3(laminateLength / 2.0f / inchesPerMeter, -laminateThickness / inchesPerMeter, 0.0f);

    }

    public void BuildTensileTestLaminate(GameObject laminatePrefab, Transform parent)
    {
        dataController = FindObjectOfType<DataController>();

        //build the visual representation
        //measurements in x, y, z or width, length, thickness
        float laminateWidth     = dataController.testLaminate.width;
        float laminateLength    = dataController.testLaminate.length;
        float laminateThickness = dataController.testLaminate.thickness;

        GameObject laminateInstance = (GameObject)Instantiate(laminatePrefab);
        laminateInstance.transform.SetParent(parent.transform);
        laminateInstance.transform.localPosition    = Vector3.zero;
        laminateInstance.transform.localEulerAngles = Vector3.zero;
        laminateInstance.transform.localScale = new Vector3(laminateWidth, laminateLength, laminateThickness) * ((10.0f / 3.0f) / inchesPerMeter);
        laminateInstance.name = "TestSample";

        //position Grippers
        float gripperLocation = 0.25f + laminateLength /2.0f/inchesPerMeter;
        upperGripperConf.transform.localPosition = new Vector3(0,   gripperLocation, 0); //not sure why the divide by 4 works..I think it is the length of the gripper
        lowerGripperConf.transform.localPosition = new Vector3(0,  -gripperLocation, 0);
    }
}
