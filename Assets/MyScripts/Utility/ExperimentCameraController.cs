//this is basically a first person camera view of the test setup
// transforms of the experimental setup are provided and selected depending on the test type


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentCameraController : MonoBehaviour
{
    public Transform tensileTestTarget;
    public Transform threePointBendTestTarget;
    public Transform camTransform;
    public float distance = .5f;
    public Transform tensileTestPosition;
    public Transform threePointBendTestPosition;

    private DataController dataController;

    private Camera cam;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float currentZ = 0.0f;
    private float sensivityX = .010f;
    private float sensivityY = .010f;

    Vector3 distanceToTarget;

    private Transform target;

    private void Start()
    {
        dataController = FindObjectOfType<DataController>();

        cam = Camera.main;

        //set the transforms base on the selected test type
        if (dataController.testType == TestType.threePointBendTensileTest)
        {
            camTransform = threePointBendTestPosition;
            target = threePointBendTestTarget;
        }
        else
        {
            camTransform = tensileTestPosition;
            target = tensileTestTarget;
        }

        this.transform.position = camTransform.position;
        this.transform.rotation = camTransform.rotation;


        camTransform = transform;
        distanceToTarget = cam.transform.position - target.transform.position;
        currentX = cam.transform.position.x;
        currentY = cam.transform.position.y;
        currentZ = cam.transform.position.z;

    }

    private void Update()
    {
        //user can pan a bit using holding the right mouse button and dragging around the scene
        if (Input.GetMouseButton(1))
        {
            currentX -= Input.GetAxis("Mouse X")*sensivityX;
            currentY -= Input.GetAxis("Mouse Y")*sensivityY;
        }

        //user can use the scrollwheel to zoom in and out of the test setup
        currentZ += Input.GetAxis("Mouse ScrollWheel");

    }

    private void LateUpdate()
    {
        camTransform.position = new Vector3 (currentX, currentY, currentZ);
    }

}
