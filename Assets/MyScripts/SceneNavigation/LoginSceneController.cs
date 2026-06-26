using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginSceneController : MonoBehaviour
{
    private DataController dataController;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
    }

    public void StoreFirstName(string input)
    {
        dataController.studentName[0] = input;
    }
    public void StoreLastName(string input)
    {
        dataController.studentName[1] = input;
    }

    public void SceneControllerMessage()
    {
    }
}