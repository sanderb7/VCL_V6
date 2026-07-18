//this is basically a first person camera view of the test setup
// transforms of the experimental setup are provided and selected depending on the test type


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

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
    private float lookSpeed;
    private float zoomSpeed;
    private float minZoom = 1f;
    private float maxZoom = 5f;

    private float minPitch;
    private float maxPitch;
    private float zoomDistance;

    private Transform initialTarget;

    private DataController dataController;

    private float yaw;
    private float pitch;

//    private Camera cam;
   
    private void Start()
    {
        lookSpeed = 30.0f;
        zoomSpeed = 1f;
        minPitch = -30f;
        maxPitch = 30f;

        dataController = FindAnyObjectByType<DataController>();

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
        if (Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * lookSpeed * Time.deltaTime;
            pitch -= mouseDelta.y * lookSpeed * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
        
        //--------------------------------------------------
        // Scroll wheel: move in current viewing direction
        //--------------------------------------------------
       
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) > 0.001f)
        {
            zoomDistance -= scroll * zoomSpeed;
            zoomDistance = Mathf.Clamp(zoomDistance, minZoom, maxZoom);

            transform.position = initialTarget.position - transform.forward * zoomDistance;
        }
    }
}
