using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public List<TaskGame> taskGames;
    public int currentTask;
    public SwitchTask switchTask;
    public static TaskManager instance;
    private void Start()
    {
        instance = this;
        StartCoroutine(StarterTask());
    }
   
    IEnumerator StarterTask()
    {
        print("Start Task "+(LevelManager.currenLevel+1));
        bool res = false;
        StartCoroutine(taskGames[LevelManager.currenLevel].Starter((result) => res = result));
        yield return new WaitUntil(() => res);
        LevelManager.currenLevel++;
        switchTask.starter((resf) => { });
        yield return new WaitForSeconds(0.5f);
        print("End Task " + (LevelManager.currenLevel + 1));
        SceneManager.LoadScene(LevelManager.currenLevel==4? "Questionnaire":"MainMenu");
    }
}

