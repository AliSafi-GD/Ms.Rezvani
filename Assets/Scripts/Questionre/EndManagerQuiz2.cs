using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class EndManagerQuiz2 : MonoBehaviour
{
    // Start is called before the first frame update
    //public QuestionnaireToggle[] toggles;
    public ContentAnswer[] contentAnswers;
   // public Button btnNext;
    Coroutine TimerQuiz;
    //public GameObject waiting;
    public ScrollRect scrollRect;
    public GameObject EndGame;
    public Vector2 v;
    public Text txtCurrentQuiz;
    int currentAnswer;
    public string timer;

    static EndManagerQuiz2 instance;
   

    public int CurrentAnswer
    {
        get => currentAnswer;
        set
        {
            currentAnswer = value;
            //txtCurrentQuiz.text = $"{value + 1} / 12 ";
        }
    }

    private void Start()
    {
        instance = this;
        foreach (var item in contentAnswers)
        {
            item.SetTgls(Next);
        }
        TimerQuiz = StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        int t = 0;
        //string time1 = "";
        while (true)
        {
            yield return new WaitForSeconds(1);
            t++;
            timer = $"{(t / 60).ToString("00")} : {(t % 60).ToString("00")}";
            // print(time1);
            // DB.instance.SetDataToQuestion2("timer", time1);
            DB.instance.data.quizs[1].timer = this.timer;
        }
    }
 
  
    public void Next()
    {
 
           
        if (CurrentAnswer >= 11)
        {
            gameObject.SetActive(false);
            EndGame.SetActive(true);
                //btnNext.gameObject.SetActive(true);
                return;
        }
           
        
        CurrentAnswer++;
        v.y += 223.4f;
        //print((toggles[CurrentAnswer].GetComponent<RectTransform>().sizeDelta.y / 2));
        StartCoroutine(INext(0.2f));

    }
    public void Back()
    {
        if (CurrentAnswer <= 0)
            return;
        CurrentAnswer--;
        v.y -= 223.4f;
        StartCoroutine(INext(0));

    }
    public void End()
    {
        StopCoroutine(TimerQuiz);
        
        //FindObjectOfType<ConnectToServer>().Send(waiting);
    }
  
    IEnumerator INext(float sc)
    {
        yield return new WaitForSeconds(sc);
        while (scrollRect.content.anchoredPosition != v)
        {
            scrollRect.content.anchoredPosition = Vector2.Lerp(scrollRect.content.anchoredPosition, v, 10 * Time.deltaTime);
            if (Vector2.Distance(scrollRect.content.anchoredPosition, v) < 1)
                scrollRect.content.anchoredPosition = v;
            yield return null;
        }

    }

    [Serializable]
    public class ContentAnswer
    {
        public GameObject objContent;
        public Toggle[] tgls;

        public void SetTgls(UnityAction action)
        {
            tgls = objContent.GetComponentsInChildren<Toggle>();
            foreach (var item in tgls)
            {

                item.onValueChanged.AddListener((res) =>
                {
                    if (res)
                    {
                        var sible = item.transform.GetSiblingIndex();
                        DB.instance.data.quizs[1].quiz[instance.CurrentAnswer] = sible + 1;
                        action.Invoke();
                        
                    }

                });
            }
        }
    }

}
