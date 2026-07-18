using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneCanvasManager : MonoBehaviour
{
   
    [SerializeField]
    GameObject sampleDesignButton;
    
    DataController dataController;
    public void Start()
    {
        dataController = FindAnyObjectByType<DataController>();

        if(dataController.testType == TestType.charpyTest)
        {
            sampleDesignButton.SetActive(false);
        }
    }   
}
