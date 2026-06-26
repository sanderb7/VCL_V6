using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]

public class ToggleButton : MonoBehaviour, IPointerEnterHandler
{

    public AudioClip sound;
    private Button button { get { return GetComponent<Button>(); } }
    //private AudioSource source = VisualizerManager._audioSource;
    private AudioSource source { get { return GetComponent<AudioSource>(); } }

    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;
        source.volume = 0.25f;
//        button.onClick.AddListener(() => PlaySound());
      
    }

 
    public void OnPointerEnter(PointerEventData eventData)
    {
        source.PlayOneShot(sound);
 //       Debug.Log("played sound");
    }
  

    void PlaySound()
    {
        source.PlayOneShot(sound);
    }
  
 
}
