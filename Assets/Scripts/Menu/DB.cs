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

    // --- سیستم بازخورد ذخیره‌سازی ---
    public System.Action<bool, string> onDataSent; // (موفق?, پیام)
    public GameObject panelSaveSuccess;
    public GameObject panelSaveFailed;
    public Text txtSaveMessage;
    public float autoHideDelay = 2.5f;

    // --- سیستم GUI پشتیبان (بدون نیاز به UI) ---
    string guiMessage = "";
    Color guiMessageColor = Color.white;
    float guiMessageTimer = 0f;
    float guiMessageDuration = 0f;
    GUIStyle guiStyle;
    GUIStyle guiBgStyle;

    // --- جلوگیری از ارسال همزمان (Race Condition Fix) ---
    bool isSending = false;
    
    // ذخیره snapshot + callback در صف (نه فقط callback)
    struct QueuedSend
    {
        public Data snapshot;
        public System.Action<bool, string> callback;
    }
    Queue<QueuedSend> sendQueue = new Queue<QueuedSend>();
    int sendCounter = 0; // برای unique ID هر ریکوئست

    public void SendData()
    {
        SendDataWithSnapshot(null);
    }
    
    public void SendDataWithCallback(System.Action<bool, string> callback)
    {
        // برای backward compatibility
        SendDataWithSnapshot(callback);
    }

    void SendDataWithSnapshot(System.Action<bool, string> callback)
    {
        // Snap-shot از دیتا در همین لحظه (نه reference) - این snapshot برای همیشه ثابت می‌مونه
        var snapshot = CloneDataForSafety();
        
        if (isSending)
        {
            // snapshot رو همراه callback ذخیره کن
            sendQueue.Enqueue(new QueuedSend { snapshot = snapshot, callback = callback });
            ShowSaveFeedback(false, "⏳ در صف ارسال...");
            return;
        }
        
        StartCoroutine(SyncSendDataWithSnapshot(snapshot, callback));
    }

    /// <summary>
    /// کپی عمیق از دیتا برای جلوگیری از Race Condition
    /// </summary>
    Data CloneDataForSafety()
    {
        var clone = new Data();
        
        // Copy InfoAccount
        clone.infoAccount = new InfoAccount();
        clone.infoAccount.userName = data.infoAccount.userName;
        clone.infoAccount.gender = data.infoAccount.gender;
        clone.infoAccount.education = data.infoAccount.education;
        clone.infoAccount.major = data.infoAccount.major;
        clone.infoAccount.mail = data.infoAccount.mail;
        clone.infoAccount.city = data.infoAccount.city;
        clone.infoAccount.age = data.infoAccount.age;
        clone.infoAccount.marital = data.infoAccount.marital;
        clone.infoAccount.job = data.infoAccount.job;
        clone.infoAccount.mainTimer = data.infoAccount.mainTimer;
        clone.infoAccount.coin = data.infoAccount.coin;
        clone.infoAccount.score = data.infoAccount.score;
        
        // Copy GamesInfo
        clone.gamesInfo = new GameInfo[4];
        for (int g = 0; g < 4; g++)
        {
            clone.gamesInfo[g] = new GameInfo(
                data.gamesInfo[g].nameof,
                data.gamesInfo[g].number,
                (int[])data.gamesInfo[g].options.Clone()
            );
            clone.gamesInfo[g].gameTimer = data.gamesInfo[g].gameTimer;
            if (data.gamesInfo[g].minigameTimer != null)
                clone.gamesInfo[g].minigameTimer = (string[])data.gamesInfo[g].minigameTimer.Clone();
        }
        
        // Copy Quizs
        clone.quizs = new Quiz[2];
        for (int q = 0; q < 2; q++)
        {
            clone.quizs[q] = new Quiz((int[])data.quizs[q].quiz.Clone());
            clone.quizs[q].timer = data.quizs[q].timer;
        }
        
        return clone;
    }

    IEnumerator SyncSendDataWithSnapshot(Data snapshot, System.Action<bool, string> callback)
    {
        isSending = true;
        sendCounter++;
        int currentSendId = sendCounter;
        
        var json = JsonConvert.SerializeObject(snapshot);
        var path = "https://script.google.com/macros/s/AKfycbxeEnyjYUmnkHVtrRQkYzRAzUGktxbRWFrfvTtU5X52lMAn0HCTjRN8ffmHbRDCPGuN/exec";
        WWWForm form = new WWWForm();
        form.AddField("json", json);
        form.AddField("id", currentSendId.ToString());
        form.AddField("timestamp", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        var www = UnityWebRequest.Post(path, form);

        yield return www.SendWebRequest();

        bool success = !www.isNetworkError && !www.isHttpError;
        string message;

        if (success)
        {
            message = "✅ اطلاعات با موفقیت ذخیره شد";
            Debug.Log($"Send #{currentSendId} - Data sent to Google Sheets successfully. Response: " + www.downloadHandler.text);
        }
        else
        {
            message = "❌ خطا در ذخیره اطلاعات: " + www.error;
            Debug.LogError($"Send #{currentSendId} - Google Sheets Error: " + www.error);
        }

        // نمایش بازخورد بصری
        ShowSaveFeedback(success, message);

        // اطلاع به سایر اسکریپت‌ها
        onDataSent?.Invoke(success, message);
        callback?.Invoke(success, message);
        
        isSending = false;
        
        // ارسال بعدی در صف (اگر باشد)
        if (sendQueue.Count > 0)
        {
            var nextSend = sendQueue.Dequeue();
            StartCoroutine(SyncSendDataWithSnapshot(nextSend.snapshot, nextSend.callback));
        }
    }

    void ShowSaveFeedback(bool success, string message)
    {
        // --- روش 1: UI (اگر تنظیم شده باشد) ---
        if (panelSaveSuccess != null)
            panelSaveSuccess.SetActive(success);
        
        if (panelSaveFailed != null)
            panelSaveFailed.SetActive(!success);

        if (txtSaveMessage != null)
        {
            txtSaveMessage.text = message;
            txtSaveMessage.gameObject.SetActive(true);
        }

        if (txtSaveMessage != null || panelSaveSuccess != null)
            StartCoroutine(HideFeedback());

        // --- روش 2: GUI پشتیبان (همیشه کار می‌کند) ---
        guiMessage = message;
        guiMessageColor = success ? Color.green : Color.red;
        guiMessageTimer = 0f;
        guiMessageDuration = autoHideDelay;
    }

    IEnumerator HideFeedback()
    {
        yield return new WaitForSeconds(autoHideDelay);
        
        if (txtSaveMessage != null)
            txtSaveMessage.gameObject.SetActive(false);
        
        if (panelSaveSuccess != null)
            panelSaveSuccess.SetActive(false);
        
        if (panelSaveFailed != null)
            panelSaveFailed.SetActive(false);
    }

    // --- GUI Overlay (بدون نیاز به هیچ UIای) ---
    void OnGUI()
    {
        if (guiMessageDuration <= 0f || guiMessageTimer >= guiMessageDuration)
            return;

        guiMessageTimer += Time.deltaTime;

        // مقدار محو شدن (fade out) در 0.5 ثانیه آخر
        float alpha = 1f;
        float fadeTime = 0.5f;
        if (guiMessageTimer > guiMessageDuration - fadeTime)
        {
            alpha = (guiMessageDuration - guiMessageTimer) / fadeTime;
        }

        // مقدار حرکت به بالا
        float offsetY = Mathf.Lerp(0, -30, guiMessageTimer / guiMessageDuration);

        // استایل متن
        if (guiStyle == null)
        {
            guiStyle = new GUIStyle();
            guiStyle.fontSize = Mathf.RoundToInt(Screen.height * 0.035f);
            guiStyle.fontStyle = FontStyle.Bold;
            guiStyle.alignment = TextAnchor.MiddleCenter;
            guiStyle.normal.textColor = Color.white;

            guiBgStyle = new GUIStyle();
            guiBgStyle.normal.background = MakeTexture(2, 2, new Color(0, 0, 0, 0.7f));
        }

        guiStyle.normal.textColor = new Color(guiMessageColor.r, guiMessageColor.g, guiMessageColor.b, alpha);

        // موقعیت: وسط صفحه، کمی پایین‌تر از مرکز
        float boxWidth = Screen.width * 0.7f;
        float boxHeight = Screen.height * 0.08f;
        float x = (Screen.width - boxWidth) / 2f;
        float y = (Screen.height / 2f) + offsetY;

        // پس‌زمینه
        GUI.Box(new Rect(x, y, boxWidth, boxHeight), "", guiBgStyle);
        // متن
        GUI.Label(new Rect(x, y, boxWidth, boxHeight), guiMessage, guiStyle);
    }

    Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
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
