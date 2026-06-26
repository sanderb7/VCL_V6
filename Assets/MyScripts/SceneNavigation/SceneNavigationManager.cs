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
        SceneManager.LoadScene("MainMenuScene");
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
            SampleDesignMenu();
        }

    }

    public void TakeAScreenShot()
    {
        dataController.TakeScreenShot();
    }
    public void SampleDesignMenu()
    {
        SceneManager.LoadScene("SampleDesignMenu");
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
    //   public void LoadTestScene(TestType testType)
    {
        //for (int i = 0; i < dataController.testLaminate.numberOfLayers; i++)
        //{
        //    laminateMechanics.NonPrincipalProperties(i, dataController);
        //}
        dataController.testType = (TestType)testType;
        //        dataController.testType = testType;
        SceneManager.LoadScene("TestLab");
    }
    public void ExitApplication()
    {
//        SceneManager.LoadScene("ExitScene");
        Application.Quit();
    }
}
