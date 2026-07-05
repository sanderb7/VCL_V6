using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneCanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField]
    //GameObject charpyTestUI;
    [SerializeField]
    GameObject sampleDesignButton;
    
    DataController dataController;
    public void Start()
    {
        //charpyTestUI.SetActive(false);
        dataController = FindObjectOfType<DataController>();

        if(dataController.testType == TestType.charpyTest)
        {
            //charpyTestUI.SetActive(true);
            sampleDesignButton.SetActive(false);
        }
    }   
}
