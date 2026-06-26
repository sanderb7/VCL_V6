using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceAnswers : MonoBehaviour
{
    public GameObject questionPanel;

    private Animator animator;

    // Start is called before the first frame update

    private void Start()
    {
        if (this.GetComponent<Animator>() != null)
        {
            animator = this.GetComponent<Animator>();
            Debug.Log("AnimationComponentFound");
        }
    }

    public void QuestionTextOn()
    {
        if (this.GetComponent<Animator>() != null) animator.speed = 0.0f;
            
        questionPanel.SetActive(true);

    }
    public void QuestionTextOff()
    {

        if (this.GetComponent<Animator>() != null) animator.speed = 0.5f;
           
        questionPanel.SetActive(false);
    }
}
