using UnityEngine;
using System.Collections;
using System;


public class MiniTask : MonoBehaviour
{
    public bool isActive=true;
    public bool isComlete;
    public bool needSwitch=true;
    public bool startTimer = false;
    public int timerElementIndex;
    Coroutine timerGame;
    private void Start()
    {
       
    }
    IEnumerator Timer()
    {
        //yield return new WaitUntil(() => enabled);
        int t = 0;
        //string time1 = "";
        while (true)
        {
            yield return new WaitForSeconds(1);
            t++;
            DB.instance.data.gamesInfo[LevelManager.currenLevel].minigameTimer[timerElementIndex] = $"{(t / 60).ToString("00")} : {(t % 60).ToString("00")}";

        }
    }
    public void StopTimer()
    {
        StopCoroutine(timerGame);
    }
    public virtual IEnumerator Starter(Action<bool> result)
    {
        yield return new WaitUntil(() => gameObject.activeInHierarchy);
        if (startTimer)
            timerGame = StartCoroutine(Timer());
        gameObject.SetActive(true);
        yield return new WaitUntil(() => isComlete);
        gameObject.SetActive(isActive);
        if(startTimer)
            StopTimer();
        result(true);
    }
}
