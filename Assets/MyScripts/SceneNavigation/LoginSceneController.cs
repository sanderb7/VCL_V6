using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginSceneController : MonoBehaviour
{
    public TMP_InputField firstName;
    public TMP_InputField lastName;

    private DataController dataController;

    private string playerName;
    private string playerFirstName = null;
    private string playerLastName = null;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindAnyObjectByType<DataController>();
        {
            if (dataController.studentName[0] != null)
            {
                firstName.text = dataController.studentName[0];
            }

            if (dataController.studentName[1] != null)
            {
                lastName.text = dataController.studentName[1];
            }
        }
    }
    public void LoginSceneCheck()
    {

        //this checks to make sure the user has entered a first and last name - that is the first part
        //the second part covers if the user deleted a name so it would come up as blank but not null
        //        if ((gameManager.studentName[0] != null && gameManager.studentName[1] != null) && (gameManager.studentName[0] != "" && gameManager.studentName[1] != ""))

        if (playerFirstName == null || playerLastName == null) return;
        // if (!erauStudentYes.isOn && !erauStudentNo.isOn) return;

        //establish new player if login name is new
        if (dataController.studentName[0] != playerFirstName || dataController.studentName[1] != playerLastName)
        {
            //totally goofy how I set the name..needs to be corrected
            dataController.studentName[0] = playerFirstName;
            dataController.studentName[1] = playerLastName;

        }
        
        dataController.SetUpFileOutputDirectories();
        dataController.StoreLoginName();

        //SceneManager.LoadScene("CopyRightsMessage");
        SceneManager.LoadScene("MainMenu");
    }
    public void StoreFirstName(string input)
    {
        dataController.studentName[0] = input;
        playerFirstName = input;
    }
    public void StoreLastName(string input)
    {
        //dataController.studentName[1] = input;
        playerLastName = input;
    }

    public void SceneControllerMessage()
    {
    }
}