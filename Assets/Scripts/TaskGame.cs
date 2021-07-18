using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGame : MonoBehaviour
{
    //public SwitchTask switchTask;
    public bool autoStart;
    
    //private void Start()
    //{
    //    if (autoStart)
    //        StartCoroutine(Starter((res) => { }));
    //}
    public List<MiniTask> miniTasks;
    public int numberTask;
    public bool isComlete;
    Coroutine timerGame;
    private void Start()
    {
        timerGame = StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        int t = 0;
        //string time1 = "";
        while (true)
        {
            yield return new WaitForSeconds(1);
            t++;
            DB.instance.data.gamesInfo[LevelManager.currenLevel].gameTimer = $"{(t / 60).ToString("00")} : {(t % 60).ToString("00")}";

        }
    }
    public void StopTimer()
    {
        StopCoroutine(timerGame);
    }
    public virtual IEnumerator Starter(Action<bool> result)
    {
        gameObject.SetActive(true);
        //print("Start Task " + name);
        foreach (var item in miniTasks)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in miniTasks)
        {
            if (!item.isComlete)
            {
                bool b = false;
                //print("mini");
                item.gameObject.SetActive(true);
                StartCoroutine(item.Starter((res) => { b = res; }));
                yield return new WaitUntil(() => b);
                if (item.needSwitch)
                {
                    TaskManager.instance.switchTask.starter((res) => { });
                    yield return new WaitForSeconds(0.5f);
                }
                item.gameObject.SetActive(false);
            }
           
          
           
            numberTask++;
            
        }
        
        print("End Task " + name);
        StopCoroutine(timerGame);
        result(true);
    }

}
