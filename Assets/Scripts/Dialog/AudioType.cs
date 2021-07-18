using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioType : MonoBehaviour
{
    public static AudioSource audioSource;
    private static bool audioType;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
    }
    public static bool PlayAudioType
    {
        get
        {
            return audioType;
        }
        set
        {
            audioType = value;
            
            switch (value)
            {
                case true:
                    audioSource.Play();
                    break;
                case false:
                    audioSource.Stop();
                    break;
            }
        }
    }
}
