# راهنمای اتصال Google Sheets به بازی Ms.Rezvani

این راهنما به شما کمک می‌کند تا به جای سرور قدیمی، داده‌های بازی را در Google Sheets ذخیره کنید.

---

## مرحله ۱: ساخت Google Sheet

1. به سایت [sheets.google.com](https://sheets.google.com) بروید
2. روی **+ Blank** کلیک کنید تا یک شیت جدید بسازید
3. اسم شیت را مثلاً `Ms.Rezvani Data` بگذارید

---

## مرحله ۲: اضافه کردن Apps Script

1. در Google Sheet، از منوی بالا بروید به:
   **Extensions → Apps Script**

   ![Extensions > Apps Script](https://developers.google.com/apps-script/images/google-sheets-menus-extensions-pointing-to-apps-script.png)

2. یک صفحه جدید با نام `Code.gs` باز می‌شود
3. **تمام کد پیش‌فرض را پاک کنید**
4. محتوای فایل `Code.gs` که در پروژه قرار داده شده را **کاملاً کپی کنید** و جایگزین کنید

---

## مرحله ۳: Deploy کردن Web App

1. روی دکمه **Deploy** (سبز رنگ، بالا سمت راست) کلیک کنید
2. گزینه **New Deployment** را انتخاب کنید
3. در پنجره باز شده:
   - **Select type**: `Web app` را انتخاب کنید
   - **Description**: مثلاً `Ms.Rezvani Data Collector` بنویسید
   - **Execute as**: `Me` (خودتان) را انتخاب کنید
   - **Who has access**: `Anyone` را انتخاب کنید (مهم!)
4. دکمه **Deploy** را بزنید
5. در پنجره بعدی، روی **Authorize access** کلیک کنید
   - اکانت گوگل خودتان را انتخاب کنید
   - روی **Advanced** کلیک کنید
   - روی **Go to [your project] (unsafe)** کلیک کنید
   - اجازه‌های دسترسی را قبول کنید
6. بعد از تأیید، یک **Web App URL** به شما داده می‌شود مثل:
   ```
   https://script.google.com/macros/s/abcdef123456789/exec
   ```
7. **این آدرس را کپی کنید** - در مرحله بعد نیاز دارید

---

## مرحله ۴: تنظیم آدرس در Unity

1. فایل `Scripts/Menu/DB.cs` را در Unity باز کنید
2. خط زیر را پیدا کنید:
   ```csharp
   var path = "https://script.google.com/macros/s/YOUR_SCRIPT_ID/exec";
   ```
3. به جای `YOUR_SCRIPT_ID`، آدرس کامل Web App خود را قرار دهید:
   ```csharp
   var path = "https://script.google.com/macros/s/abcdef123456789/exec";
   ```

---

## مرحله ۵: تست کردن

1. بازی را در Unity اجرا کنید (Play)
2. دکمه Minimize را بزنید یا هر جایی که `DB.instance.SendData()` فراخوانی می‌شود
3. به Google Sheet خود برگردید
4. سه شیت جدید باید ایجاد شده باشند:
   - **InfoAccount** - اطلاعات کاربران
   - **GamesInfo** - اطلاعات بازی‌ها
   - **Quiz1** و **Quiz2** - نتایج پرسشنامه‌ها

---

## ساختار شیت‌ها

### شیت InfoAccount
| Timestamp | UserName | Gender | Education | Major | Mail | City | Age | Marital | Job | Coin | Score | MainTimer |
|-----------|----------|--------|-----------|-------|------|------|-----|---------|-----|------|-------|-----------|
| تاریخ ثبت | نام کاربر | جنسیت | تحصیلات | رشته | ایمیل | شهر | سن | تأهل | شغل | سکه | امتیاز | زمان کل |

### شیت GamesInfo
| Timestamp | UserName | GameNumber | GameTimer | MiniGameTimers | Options |
|-----------|----------|------------|----------|---------------|---------|
| تاریخ ثبت | نام کاربر | شماره بازی | زمان بازی | زمان‌های زیربازی | پاسخ‌ها |

### شیت Quiz1 / Quiz2
| Timestamp | UserName | Timer | Q1 | Q2 | Q3 | ... |
|-----------|----------|-------|----|----|----|-----|
| تاریخ ثبت | نام کاربر | زمان | پاسخ۱ | پاسخ۲ | پاسخ۳ | ... |

---

## نکات مهم

⚠️ **هر بار که دکمه Minimize زده می‌شود (`DB.instance.SendData()`)، یک رکورد جدید اضافه می‌شود**

✅ اگر خطایی رخ دهد، در کنسول Unity (Window → General → Console) خطاها نمایش داده می‌شود

✅ برای تست سریع‌تر، می‌توانید در ادیتور یونیتی هم اجرا کنید - دیتا فرستاده می‌شود

⛔ اگر Web App را مجدداً Deploy کنید و گزینه **New Version** را انتخاب کنید، **URL تغییر نمی‌کند**، فقط اگر **New Deployment** بزنید URL عوض می‌شود