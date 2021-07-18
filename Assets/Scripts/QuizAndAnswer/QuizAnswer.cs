
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuizAnswer : MiniTask
{
    public Text txtQuiz;
    public Button[] btnsAnswer;
   
    [TextArea]
    public string strDialogue;
    public string[] arr;
    public bool endType;

    [Header("Values DB")]
    public int numberElementDB;
    public int[] valueDB = new int[4] { 0, 1, 2, 3 };
    public int valueSelected;
    public override IEnumerator Starter(Action<bool> result)
    {
        
        arr = strDialogue.Split('\n');
        //txtQuiz.text = arr[0];
        bool b = false;
        TypeDialog.instance.Starter((res) => b=res, txtQuiz, arr[0]);
        yield return new WaitUntil(() => b);
        for (int i = 1; i < arr.Length; i++)
        {
            b = false;

            //btnsAnswer[i-1].transform.GetChild(0).GetComponent<Text>().text = arr[i];
            var n = 0;
            TypeDialog.instance.Starter((res) => b = res, btnsAnswer[i - 1].transform.GetChild(0).GetComponent<Text>(), arr[i]);
            
            n++;
            yield return new WaitUntil(() => b);
        }
        foreach (var item in btnsAnswer)
        {
            item.onClick.AddListener(() => {
                if (endType)
                {
                    result(true);
                    print("click btn ");
                    
                    gameObject.SetActive(isActive);
                    valueSelected = valueDB[btnsAnswer.ToList().IndexOf(item)];
                    DB.instance.data.gamesInfo[LevelManager.currenLevel].SetValueOptions(numberElementDB, valueSelected);
                }

            });
        }
        yield return new WaitForSeconds(1);
        endType = true;
        //print("Current Game " + LevelManager.currenLevel+"   value  "+ valueSelected);
       
        //return base.Starter(result);
    }

}


