using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lvl3 : TaskGame
{
    public string answerUser;
    public Button btnEnd;
    public MiniTask[] skips;
    public QuizAnswer quiz;
    //private void Start()
    //{
    //    btnEnd.onClick.AddListener(() => {
    //        isComlete = true;
    //    });
    //}
    private void Update()
    {
        btnEnd.interactable = answerUser == "" ? false : true;
    }
    public void SetChar(string index)
    {
        answerUser = index;
    }
    public void SkipTasks()
    {
        foreach (var item in skips)
        {
            item.isComlete = true;
        }
    }
    public void ChangeText()
    {
        quiz.strDialogue = quiz.strDialogue.Insert(7, answerUser);
    }

}
