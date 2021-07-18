using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;

    public AudioClip btnSound;
    public Button[] btns;
    private void Start()
    {
        foreach (var item in btns)
        {
            item.onClick.AddListener(PlayButton);
        }
    }
    public void PlayButton()
    {
        source.PlayOneShot(btnSound);
    }
        
    
}
