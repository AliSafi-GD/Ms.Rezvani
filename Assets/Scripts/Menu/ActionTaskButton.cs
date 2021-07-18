using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionTaskButton : MonoBehaviour
{
    public Button btn;
    public void starter(Action<bool> result)
    {
        btn.onClick.AddListener(() => result(true));
    }
    


}
