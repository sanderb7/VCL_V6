using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProcessScript : MonoBehaviour
{
    public GameObject[] processStep = new GameObject[4];
    public float[] processStepTime = new float[4];
    public Material[] materials = new Material[2];
    public TextMeshProUGUI totalProcessTimeDisplay;
    public GameObject finalMessageDisplay;
    public AudioSource audioSource;
    public AudioClip[] audioClips = new AudioClip[3];



    public void Start()
    {
        finalMessageDisplay.SetActive(false);
        StartCoroutine(ProcessLaminate());      
    }

  
    IEnumerator ProcessLaminate()
    {
        float totalProcessTime = 0.0f;

        Image blockImage;
        for (int i = 0; i < processStep.Length; i++)
        {
            totalProcessTime += processStepTime[i];
        }

        for (int i = 0; i < processStep.Length; i++)
        {
            blockImage = processStep[i].GetComponent<Image>();
            blockImage.material = materials[0];
            for (int j = 0; j < processStepTime[i]; j++)
            {
                yield return new WaitForSeconds(1);
                audioSource.clip = audioClips[0];
                audioSource.Play();
                totalProcessTime--;
                totalProcessTimeDisplay.text = totalProcessTime.ToString();
            }
            blockImage.material = materials[1];
            audioSource.clip = audioClips[1];
            audioSource.Play();
            //include a sound ding
        }
        finalMessageDisplay.SetActive(true);
        audioSource.clip = audioClips[2];
        audioSource.Play();

    }
}
