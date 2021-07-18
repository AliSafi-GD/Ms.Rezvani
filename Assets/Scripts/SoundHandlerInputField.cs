
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandlerInputField : MonoBehaviour
{
    public GameObject objSoundType;

    public void SFXWithPitch(AudioClip audioClip)
    {
        CreateSFX_EditPitch(audioClip);
    }
    public void SFXWithOutPitch(AudioClip audioClip)
    {
        CreateSFX(audioClip);
    }


    void CreateSFX(AudioClip audioClip)
    {
        var sound = Instantiate(objSoundType).GetComponent<AudioSource>();
        sound.clip = audioClip;
        sound.Play();
        Destroy(sound.gameObject, sound.clip.length);
    }
    void CreateSFX_EditPitch(AudioClip audioClip)
    {
        var sound = Instantiate(objSoundType).GetComponent<AudioSource>();
        sound.clip = audioClip;
        sound.pitch = Random.Range(0.7f, 1.4f);
        sound.Play();
        Destroy(sound.gameObject, sound.clip.length);
    }
}
