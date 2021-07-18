using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TypeDialog : MonoBehaviour
{
    public float speedType;
    public static TypeDialog instance;
    private void Awake()
    {
        instance = this;
    }
    public void Starter(Action<bool> result,Text txt,string content) {
        
        StartCoroutine(_Typing((r) => {
            result(r);
        },txt,content));
        
       
    }
    public IEnumerator _Typing(Action<bool> result,Text txt,string content){
        AudioType.PlayAudioType = true;
        var s = "";
        for (int i = 0; i < content.Length; i++)
        {
            
            s += content[i];
            txt.text = s;
            yield return new WaitForSeconds(speedType);
            if (Input.GetMouseButton(0))
            {
                txt.text = content;
                break;
            }
           
        }
        AudioType.PlayAudioType = false;
        yield return new WaitForEndOfFrame();
        //objDialog.SetActive(false);
        result(true);
    }
    
}
