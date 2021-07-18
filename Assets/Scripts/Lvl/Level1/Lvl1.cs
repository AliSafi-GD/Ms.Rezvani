
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1 : TaskGame
{
    public Animator anim;

    public GameObject objRobot;
    
    public override IEnumerator Starter(Action<bool> result)
    {
        
    
        gameObject.SetActive(true);
        //print("Start Task " + name);
        foreach (var item in miniTasks)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in miniTasks)
        {
            bool complete = false;
            //switchTask.starter((res) => complete = res);
            //yield return new WaitUntil(() => complete);

            //complete = false;
           
            item.gameObject.SetActive(true);
            StartCoroutine(item.Starter((res) => complete = res));
            yield return new WaitUntil(() => complete);
            if (item.needSwitch)
                TaskManager.instance.switchTask.starter((res) => { });
            yield return new WaitForSeconds(0.5f);
            //item.gameObject.SetActive(false);
            numberTask++;

        }
        MoveToShip();
        yield return new WaitForSeconds(1);
        //print("End Task " + name);
        StopTimer();
        result(true);


       
    }
    
    public void RotateRobot()
    {
        objRobot.SetActive(false);
        anim.Play("Rotate",1);
    }
    public void MoveToShip()
    {
        anim.Play("MoveToShip",1);
    }
}
