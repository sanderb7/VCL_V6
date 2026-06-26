using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayStudentName : MonoBehaviour
{
    private DataController dataController;

    public GameObject studentName;


    // Start is called before the first frame update
    void Start()
    {
        dataController         = FindObjectOfType<DataController>();

        if (studentName != null)
           DisplayName();

    }

    private void DisplayName()
    {
        TextMeshProUGUI studentNameTextBlock = studentName.GetComponent<TextMeshProUGUI>();
        studentNameTextBlock.text = dataController.studentName[0] + " " + dataController.studentName[1];
    }

    public void SceneControllerMessage()
    {
    }

}
