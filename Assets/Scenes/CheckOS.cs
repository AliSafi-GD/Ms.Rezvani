using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckOS : MonoBehaviour
{

    public GameObject checkMaximize;


    private void Update()
    {
        checkMaximize.SetActive(!Screen.fullScreen);
    }
    public void MuteAudio(bool b)
    {
        AudioListener.volume = b ? 0 : 1;
    }

}
