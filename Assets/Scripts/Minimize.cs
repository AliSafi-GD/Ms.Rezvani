using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimize : MonoBehaviour
{
    public void Mini()
    {
        Screen.fullScreen = false;
        DB.instance.StopTimer();
        DB.instance.SendData();
    }
}
