using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class EndManagerQuiz1 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public ContentAnswer[] contentAnswer;
    public Button btnNext;
    public int totalScore;
    public Text txtResult;
    public GameObject endQuiz;
    Coroutine TimerQuiz;
    public ScrollRect scrollRect;
    public Vector2 v;
    public string t;
   // public Text txtCurrentQuiz;
    public int currentAnswer;
    public int currentSible;
    static EndManagerQuiz1 instance;
    public int CurrentAnswer
    {
        get => currentAnswer;
        set
        {
            currentAnswer = value;
            //txtCurrentQuiz.text = $"{value+1} / 21 ";
        }
    }

    private void Start()
    {
        instance = this;
        foreach (var item in contentAnswer)
        {
            item.SetTgls(Next,AddScore);
        }
        TimerQuiz = StartCoroutine(Timer());
        
    }
    IEnumerator Timer()
    {
        int t = 0;
        
        while (true)
        {
            yield return new WaitForSeconds(1);
            t++;
            this.t = $"{(t / 60).ToString("00")} : {(t % 60).ToString("00")}";
            DB.instance.data.quizs[0].timer = this.t;
           // print(time1);
            //DB.instance.SetDataToQuestion1("timer", time1);
        }
    }
    public void SaveTime()
    {
        StopCoroutine(TimerQuiz);
    }
    public void AddScore(int s)
    {

        totalScore += s;
        currentSible = s;
    }
   
    public void TotalScore()
    {
        //var dic = DB.instance.infoAccount.question1;
        //foreach (var item in dic)
        //{
        //    print(item.Value);
        //    if (item.Key == "timer")
        //        continue;
        //    totalScore += Int32.Parse(item.Value);
        //}
        endQuiz.SetActive(true);
        if (totalScore <= 9)
        {
            txtResult.text = "هیچ یا کمترین افسردگی";
        }
        else if (totalScore <= 13 && totalScore >= 10)
        {
            txtResult.text = "افسردگی خفیف";
        }
        else if (totalScore <= 20 && totalScore >= 14)
        {
            txtResult.text = "افسردگی متوسط";
        }
        else if (totalScore >= 21)
        {
            txtResult.text = "افسردگی شدید";
        }

    }
    public void Next()
    {
        if (CurrentAnswer >= 14)
        {
            //btnNext.gameObject.SetActive(true);
            TotalScore();
            return;
            
        }
            
        CurrentAnswer++;
        print("Next"+currentAnswer);
        v.y += 223.4f;
        //print((toggles[CurrentAnswer].GetComponent<RectTransform>().sizeDelta.y / 2));
        StartCoroutine(INext(0.5f));
        
    }
    public void Back()
    {
        if (CurrentAnswer <= 0)
            return;
        CurrentAnswer--;
        v.y -= 223.4f;
        totalScore -= contentAnswer[CurrentAnswer].scoreSelected;
        StartCoroutine(INext(0));

    }
  
    IEnumerator INext(float sec)
    {
        yield return new WaitForSeconds(sec);
        while (scrollRect.content.anchoredPosition != v)
        {
            scrollRect.content.anchoredPosition = Vector2.Lerp(scrollRect.content.anchoredPosition, v, 5 * Time.deltaTime);
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
        public int scoreSelected;
        public void SetTgls(UnityAction action, UnityAction<int> action2)
        {
            tgls = objContent.GetComponentsInChildren<Toggle>();
            foreach (var item in tgls)
            {
                
                item.onValueChanged.AddListener((res) =>
                {
                    if (res)
                    {
                        var sible = item.transform.GetSiblingIndex();
                        DB.instance.data.quizs[0].quiz[instance.CurrentAnswer] = sible;
                        action.Invoke();
                       
                        
                        print(sible);

                        scoreSelected = sible;
                        action2(sible);
                        

                    }
                    
                });
            }
        }
    }

}
