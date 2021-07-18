using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTask : MonoBehaviour
{
    Animator anim;
    GameObject objFade;
    public void starter(Action<bool> result)
    {
        //print("Switch");
        if (objFade == null)
            objFade = transform.GetChild(0).gameObject;

        gameObject.SetActive(true);
        objFade.SetActive(true);
        if (anim == null)
            anim = GetComponent<Animator>();

        //var l = anim.GetCurrentAnimatorStateInfo(0).length;

        StartCoroutine(IsFinishSwitch(r =>
        {
            result(r);
        }));
    }
    IEnumerator IsFinishSwitch(Action<bool> result)
    {
        anim.Play("Fade");
        yield return null;
        var length = anim.GetCurrentAnimatorStateInfo(0).length;
        //print(length);
        yield return new WaitForSeconds(length);
        objFade.SetActive(false);
        gameObject.SetActive(false);
        result(true);
    }
    public void Acitve()
    {
        gameObject.SetActive(false);
    }
}
