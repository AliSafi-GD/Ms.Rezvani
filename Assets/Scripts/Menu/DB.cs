using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UPersian.Utils;
using UnityEngine.UI;

public class DB : MonoBehaviour
{

    [SerializeField] public Data data;
    public static DB instance;
    public Text txtCoin;
    int coin;
    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
            txtCoin.text = value.ToString();
            data.infoAccount.coin = value;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }
    public void SetCoinAndScore(int coin,int score)
    {
        Coin += coin;
        data.infoAccount.score += score;
    }

    Coroutine timer;
    public void StartTimer()
    {
        if(timer==null)
            timer = StartCoroutine(Timer());
    }
    public void StopTimer()
    {
        StopCoroutine(timer);
    }
   IEnumerator Timer()
    {
        print("Start Main Timer");
        int t = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            t++;
            data.infoAccount.mainTimer = $"{t / 60}:{t % 60}";
        }
    }

    public void SendData()
    {

        StartCoroutine(SyncSendData());
    }
    IEnumerator SyncSendData()
    {
        var json = JsonConvert.SerializeObject(data);
        var path = "https://www.udevgame.ir/games/web-gl/index.php";
        WWWForm form = new WWWForm();
        form.AddField("json", json);
        var www =UnityWebRequest.Post(path, form);
        
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            print(www.error);
        }
        else
        {
            print(json);
            print(www.downloadHandler.text);
        }
    }
}
[Serializable]
public class Data{
    
    [SerializeField] public InfoAccount infoAccount;
    [SerializeField]
    public GameInfo[] gamesInfo = new GameInfo[4]
    {
        new GameInfo("0",0,new int[2]{-1,-1 }),
        new GameInfo("1",1,new int[5]{-1,-1 ,-1,-1,-1}),
        new GameInfo("2",2,new int[5]{-1,-1 ,-1,-1,-1}),
        new GameInfo("3",3,new int[4]{-1,-1 ,-1,-1}),
    };
    [SerializeField]
    public Quiz[] quizs = new Quiz[2]
    {
        new Quiz(new int[15]),
        new Quiz(new int[12])

    };
}
[Serializable]
public class InfoAccount
{
    public string userName, gender,education,major,mail,city,age,marital,job;
    public string mainTimer;
    public int coin = 1000, score;
    public void IsFullAllSpecification(Action<bool> result)
    {
        
        if(job.Length>0&&userName.Length>0 && gender.Length>0 && education.Length>0 && major.Length>0 && mail.Length>0 && city.Length>0 && age.Length>0 && marital.Length>0 )
        result(true);
        else
        result(false);
    }
}
[Serializable]
public class GameInfo
{
    [JsonIgnore]
    public string nameof;
    [JsonIgnore]
    public int number;
    public string gameTimer;
    public string[] minigameTimer;
    public int[] options;

    public GameInfo(string nameof, int number, int[] options)
    {
        this.nameof = nameof;
        this.number = number;
        this.options = options;
    }

    public void SetValueOptions (int element,int value)
    {
        options[element] = value;
    }
}
[Serializable]
public class Quiz
{
    public int[] quiz;
    public string timer;
    public Quiz(int[] quiz)
    {
        this.quiz = quiz;
    }
}
