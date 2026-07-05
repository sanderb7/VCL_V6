///
// This script is used for scene navigation.  It cantains methods to call scenes based on user input


using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneNavigationManager : MonoBehaviour
{
    DataController dataController;

    public void Start()
    {
        dataController = FindObjectOfType<DataController>();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoginNameCheck()
    {
        //Debug.Log(dataController.studentName[0]);
        //Debug.Log(dataController.studentName[1]);

        if (dataController.studentName[0] == null || dataController.studentName[1] == null)
        {
            dataController.studentName[0] = null;
            dataController.studentName[1] = null;
            SceneManager.LoadScene("StartUpScene");
        }
        else
        {
            dataController.SetUpFileOutputDirectories();
            MainMenu();
        }

    }

    public void TakeAScreenShot()
    {
        dataController.TakeScreenShot();
    }
    
    public void BuildTestSample()
    {

        switch (dataController.testType)
        {
            case TestType.isotropicMaterialTensileTest:
                BuildIsotropicMaterial();
                break;

            case TestType.orthotropicMaterialTensileTest:
                BuildOrthotropicMaterial();
                break;

            case TestType.multilayerMaterialTensileTest:
            case TestType.threePointBendTensileTest:
                BuildLaminateScene();
                break;
        }
    }
    public void BuildIsotropicMaterial()
    {

        SceneManager.LoadScene("BuildIsotropicTestSample");
    }

    public void BuildOrthotropicMaterial()
    {

        SceneManager.LoadScene("BuildOrthotropicTestSample");
    }

    public void BuildLaminateScene()
    {
        SceneManager.LoadScene("BuildMultiLayerTestSample");
    }

    public void LoadProcessingScene(int testType)
    {
        dataController.testType = (TestType)testType;
        SceneManager.LoadScene("ProcessingLab");
    }
    public void TestingScene()
    {
        SceneManager.LoadScene("TestLab");
    }
    public void LoadTestScene()
    { 
        SceneManager.LoadScene("TestLab");
    }
    public void LoadTestScene(int testType)    
    {
        dataController.testType = (TestType)testType;
        SceneManager.LoadScene("TestLab");
    }
    public void ExitApplication()
    {
//        SceneManager.LoadScene("ExitScene");
        Application.Quit();
    }
}
