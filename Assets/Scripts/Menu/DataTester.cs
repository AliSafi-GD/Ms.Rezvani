using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// اسکریپت تست برای بررسی اتصال Google Sheets
/// با داده‌های فیک مشابه داده‌های واقعی بازی
/// برای تست، این اسکریپت را روی یک GameObject خالی بندازید و Play کنید
/// </summary>
public class DataTester : MonoBehaviour
{
    [Header("تنظیمات تست")]
    public bool autoSendOnStart = false;
    [Tooltip("تعداد تست‌های متوالی برای ارسال")]
    public int testCount = 1;
    [Tooltip("تأخیر بین هر تست (ثانیه)")]
    public float delayBetweenTests = 1f;

    void Start()
    {
        if (autoSendOnStart)
        {
            StartCoroutine(RunTests());
        }
    }

    [ContextMenu("ارسال داده تست")]
    public void SendFakeData()
    {
        StartCoroutine(RunTests());
    }

    [ContextMenu("ارسال یک داده ساده")]
    public void SendSingleFakeData()
    {
        FillFakeData();
        DB.instance.SendData();
    }

    IEnumerator RunTests()
    {
        for (int i = 0; i < testCount; i++)
        {
            Debug.Log($"<color=yellow>Test {i + 1}/{testCount} - Sending fake data...</color>");
            
            FillFakeData();
            DB.instance.SendData();

            yield return new WaitForSeconds(delayBetweenTests);
        }

        Debug.Log($"<color=green>✅ All {testCount} tests completed! Check your Google Sheet.</color>");
    }

    /// <summary>
    /// پر کردن دیتا با داده‌های فیک مشابه داده‌های واقعی بازی
    /// </summary>
    void FillFakeData()
    {
        var data = DB.instance.data;

        // 1. اطلاعات کاربری فیک
        data.infoAccount.userName = "تست کاربر " + UnityEngine.Random.Range(100, 999);
        data.infoAccount.gender = UnityEngine.Random.Range(0, 2) == 0 ? "مرد" : "زن";
        data.infoAccount.education = GetRandomEducation();
        data.infoAccount.major = GetRandomMajor();
        data.infoAccount.mail = $"testuser{Guid.NewGuid().ToString().Substring(0, 5)}@gmail.com";
        data.infoAccount.city = GetRandomCity();
        data.infoAccount.age = UnityEngine.Random.Range(18, 45).ToString();
        data.infoAccount.marital = UnityEngine.Random.Range(0, 2) == 0 ? "مجرد" : "متأهل";
        data.infoAccount.job = GetRandomJob();
        data.infoAccount.coin = UnityEngine.Random.Range(500, 5000);
        data.infoAccount.score = UnityEngine.Random.Range(0, 1000);
        data.infoAccount.mainTimer = $"0{UnityEngine.Random.Range(1, 9)}:{UnityEngine.Random.Range(10, 59)}";

        // 2. اطلاعات بازی‌ها (4 بازی)
        // بازی 1 - 2 سؤالی
        data.gamesInfo[0].gameTimer = $"00:{UnityEngine.Random.Range(10, 59)}";
        data.gamesInfo[0].minigameTimer = new string[] { $"00:{UnityEngine.Random.Range(5, 30)}" };
        data.gamesInfo[0].options[0] = UnityEngine.Random.Range(0, 2);
        data.gamesInfo[0].options[1] = UnityEngine.Random.Range(0, 2);

        // بازی 2 - 5 سؤالی
        data.gamesInfo[1].gameTimer = $"0{UnityEngine.Random.Range(1, 5)}:{UnityEngine.Random.Range(10, 59)}";
        data.gamesInfo[1].minigameTimer = new string[] {
            $"00:{UnityEngine.Random.Range(5, 30)}",
            $"00:{UnityEngine.Random.Range(5, 30)}",
            $"00:{UnityEngine.Random.Range(5, 30)}"
        };
        for (int i = 0; i < 5; i++)
            data.gamesInfo[1].options[i] = UnityEngine.Random.Range(0, 4);

        // بازی 3 - 5 سؤالی
        data.gamesInfo[2].gameTimer = $"0{UnityEngine.Random.Range(1, 5)}:{UnityEngine.Random.Range(10, 59)}";
        data.gamesInfo[2].minigameTimer = new string[] {
            $"00:{UnityEngine.Random.Range(5, 30)}",
            $"00:{UnityEngine.Random.Range(5, 30)}"
        };
        for (int i = 0; i < 5; i++)
            data.gamesInfo[2].options[i] = UnityEngine.Random.Range(0, 4);

        // بازی 4 - 4 سؤالی
        data.gamesInfo[3].gameTimer = $"00:{UnityEngine.Random.Range(10, 59)}";
        data.gamesInfo[3].minigameTimer = new string[] {
            $"00:{UnityEngine.Random.Range(5, 30)}"
        };
        for (int i = 0; i < 4; i++)
            data.gamesInfo[3].options[i] = UnityEngine.Random.Range(0, 3);

        // 3. پرسشنامه‌ها
        // Quiz1 - 15 سؤال (افسردگی - مقادیر 0 تا 3)
        for (int i = 0; i < 15; i++)
            data.quizs[0].quiz[i] = UnityEngine.Random.Range(0, 4);
        data.quizs[0].timer = $"0{UnityEngine.Random.Range(1, 9)}:{UnityEngine.Random.Range(10, 59)}";

        // Quiz2 - 12 سؤال (مقادیر 0 تا 3)
        for (int i = 0; i < 12; i++)
            data.quizs[1].quiz[i] = UnityEngine.Random.Range(0, 4);
        data.quizs[1].timer = $"0{UnityEngine.Random.Range(1, 9)}:{UnityEngine.Random.Range(10, 59)}";

        Debug.Log($"<color=cyan>Fake data generated for user: {data.infoAccount.userName}</color>");
    }

    // ------ توابع کمکی برای داده‌های تصادفی ------

    string GetRandomEducation()
    {
        string[] options = { "دیپلم", "کارشناسی", "کارشناسی ارشد", "دکتری" };
        return options[UnityEngine.Random.Range(0, options.Length)];
    }

    string GetRandomMajor()
    {
        string[] options = { "علوم تربیتی", "روانشناسی", "علوم کامپیوتر", "مهندسی", "پزشکی", "حسابداری", "مدیریت", "هنر" };
        return options[UnityEngine.Random.Range(0, options.Length)];
    }

    string GetRandomCity()
    {
        string[] options = { "تهران", "اصفهان", "شیراز", "مشهد", "تبریز", "رشت", "اهواز", "کرمانشاه", "کرمان", "یزد" };
        return options[UnityEngine.Random.Range(0, options.Length)];
    }

    string GetRandomJob()
    {
        string[] options = { "دانشجو", "معلم", "مهندس", "دکتر", "کارمند", "آزاد", "خانه‌دار", "محقق" };
        return options[UnityEngine.Random.Range(0, options.Length)];
    }
}