//this is basically a first person camera view of the test setup
// transforms of the experimental setup are provided and selected depending on the test type


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentCameraController : MonoBehaviour
{
    public Transform cameraStart;
    [Header("Camera Locations")]
    public Transform tensileTestPosition;
    public Transform tensileTestTarget;

    public Transform threePointBendTestTarget;
    public Transform threePointBendTestPosition;

    public Transform charpyTestTarget;
    public Transform charpyTestPosition;

    [Header("Camera Controls")]
    public float lookSpeed = 120f;
    public float zoomSpeed = 3f;

    public float minPitch = -75f;
    public float maxPitch = 75f;

    private DataController dataController;

    private Transform initialTarget;

    private float yaw;
    private float pitch;

//    private Camera cam;
   
    private void Start()
    {
        dataController = FindObjectOfType<DataController>();

        Transform startPosition = tensileTestPosition;
        initialTarget = tensileTestTarget;

        //set the transforms base on the selected test type - need to work on this
        //set default to tensile test location and modify if other
        //this.cameraStart = tensileTestPosition;
        //target = tensileTestTarget;

        if (dataController.testType == TestType.threePointBendTensileTest)
        {
            startPosition = threePointBendTestPosition;
            initialTarget = threePointBendTestTarget;
        }
        else if (dataController.testType == TestType.charpyTest)
        {
            startPosition = charpyTestPosition;
            initialTarget = charpyTestTarget;
        }

        transform.position = startPosition.position;
        // Aim camera once at the initial target
        transform.LookAt(initialTarget);

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        if (pitch > 180f)
            pitch -= 360f;

    }
    
    private void Update()
    {
        //--------------------------------------------------
        // Right mouse drag: look around
        //--------------------------------------------------

        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        //--------------------------------------------------
        // Scroll wheel: move in current viewing direction
        //--------------------------------------------------

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.001f)
        {
            transform.position += transform.forward * scroll * zoomSpeed;
        }
    }
}
