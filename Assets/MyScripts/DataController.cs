//This script contains data that is persistant throught out several of the scenes
//note to self: what is the different between this an a singleton

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//setup an enum to track different test types 
public enum TestType
{
    isotropicMaterialTensileTest   = 10,
    orthotropicMaterialTensileTest = 11,
    multilayerMaterialTensileTest  = 12,
    threePointBendTensileTest      = 20,
    charpyTest                     = 30
}
public class DataController : MonoBehaviour
{
    public string[] studentName = new string[2];
    public string picturesOutputPath;
    public string testDataOutputPath;

    public LaminateProperties testLaminate; //would rather call this test sample at this point

    public TestType testType;

    private int screenshotCount = 0;

    [HideInInspector]
    public string studentUserName;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        studentName[0] = null;
        studentName[1] = null;

        if (studentUserName != null)
        {
            string storedUserName = PlayerPrefs.GetString(studentUserName);
            string[] parsedName = storedUserName.Split('.');
            if (parsedName.Length == 2)
            {
                studentName[0] = parsedName[0];
                studentName[1] = parsedName[1];
            }
        }

        //        may put an openning scene here but for now lets go with this
        SceneManager.LoadScene("StartUpScene");
    }

    //setup the directories to store user information in a desktop folder
    public void SetUpFileOutputDirectories()
    {
        string studentNamePath = studentName[0] + studentName[1];

        Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\" + studentNamePath);
        Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\" + studentNamePath + "\\Pictures");
        Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\" + studentNamePath + "\\TestData");

        picturesOutputPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop).ToString() + "\\" + studentNamePath + "\\Pictures";
        testDataOutputPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop).ToString() + "\\" + studentNamePath + "\\TestData";
    }

    public void TakeScreenShot()
    {
        string filePath;
        string screenshotFilename;
        do
        {
            screenshotCount++;
            screenshotFilename = "screenshot" + screenshotCount + ".png";
            filePath = Path.Combine(picturesOutputPath, screenshotFilename);
        } while (System.IO.File.Exists(filePath));

        ScreenCapture.CaptureScreenshot(filePath);
    }
    public void StoreLoginName()
    {
        string userName = studentName[0] + "." + studentName[1];
        PlayerPrefs.SetString(studentUserName, userName);
    }
}
