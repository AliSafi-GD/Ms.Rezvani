using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBtn : MiniTask
{
    public Button btnAccept;
    public Text[] txtDialogue;
    public GameObject[] parentTxt;
    public string[] strDialogue;
    //private void Start()
    //{
    //    StartCoroutine(Starter((res) => { }));


    //}
    public override IEnumerator Starter(Action<bool> result)
    {
        btnAccept.interactable = false;
        for (int i = 0; i < txtDialogue.Length; i++)
        {
            bool b = false;
            print("Dilague " + i);
            if(parentTxt.Length>0)
            parentTxt[i].SetActive(true);
            TypeDialog.instance.Starter((res) => b = res, txtDialogue[i], strDialogue[i]);
            
            yield return new WaitUntil(() => b);
        }
        yield return new WaitForSeconds(1);
        btnAccept.interactable = true;
        btnAccept.onClick.AddListener(() => result(true));
        //return base.Starter(result);
    }
}
