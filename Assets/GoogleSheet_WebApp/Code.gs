/**
 * Google Apps Script - دریافت و ذخیره دیتای بازی Ms.Rezvani در Google Sheets
 * 
 * نحوه استفاده:
 * 1. یک Google Sheet جدید بسازید
 * 2. Extensions -> Apps Script
 * 3. این کد را جایگزین کنید
 * 4. Deploy -> New Deployment -> Web App
 * 5. دسترسی Anyone رو بدهید و Deploy کنید
 * 6. آدرس Web App را در کد Unity قرار دهید
 */

function doGet(e) {
  return ContentService.createTextOutput(JSON.stringify({
    status: "ok",
    message: "Ms.Rezvani Data Collector is running"
  })).setMimeType(ContentService.MimeType.JSON);
}

/** 
 * Lock timeout for concurrent requests (ms)
 * Google Sheets has a lock mechanism to prevent concurrent writes
 */
var LOCK_TIMEOUT_MS = 15000;

function doPost(e) {
  try {
    const jsonString = e.parameter.json;
    const requestId = e.parameter.id || "unknown";
    const requestTimestamp = e.parameter.timestamp || "unknown";
    
    if (!jsonString) {
      return createResponse("error", "No JSON data received");
    }
    
    const data = JSON.parse(jsonString);
    
    // Lock to prevent concurrent writes (Google limitation)
    var lock = LockService.getScriptLock();
    var locked = lock.tryLock(LOCK_TIMEOUT_MS);
    
    if (!locked) {
      Logger.log("Request #" + requestId + " - FAILED: Could not acquire lock (timeout " + LOCK_TIMEOUT_MS + "ms)");
      return createResponse("error", "Server busy, please try again. Request #" + requestId);
    }
    
    try {
      const sheet = SpreadsheetApp.getActiveSpreadsheet();
      
      Logger.log("Request #" + requestId + " - Processing. Timestamp: " + requestTimestamp + " - User: " + data.infoAccount.userName);
      
      // 1. ذخیره اطلاعات کاربر
      saveInfoAccount(sheet, data);
      
      // 2. ذخیره اطلاعات بازی‌ها
      saveGamesInfo(sheet, data);
      
      // 3. ذخیره نتایج پرسشنامه‌ها
      saveQuiz(sheet, data);
      
      Logger.log("Request #" + requestId + " - SUCCESS");
      
      return createResponse("ok", "Data saved successfully. Request #" + requestId);
      
    } finally {
      lock.releaseLock();
    }
    
  } catch (err) {
    Logger.log("Request #" + (e.parameter.id || "unknown") + " - ERROR: " + err.toString());
    return createResponse("error", err.toString());
  }
}

/**
 * ذخیره اطلاعات حساب کاربری
 */
function saveInfoAccount(sheet, data) {
  const sheetName = "InfoAccount";
  let ws = sheet.getSheetByName(sheetName);
  if (!ws) {
    ws = sheet.insertSheet(sheetName);
    ws.appendRow([
      "Timestamp",
      "UserName", 
      "Gender",
      "Education",
      "Major",
      "Mail",
      "City",
      "Age",
      "Marital",
      "Job",
      "Coin",
      "Score",
      "MainTimer"
    ]);
  }
  
  const info = data.infoAccount;
  ws.appendRow([
    new Date(),
    info.userName || "",
    info.gender || "",
    info.education || "",
    info.major || "",
    info.mail || "",
    info.city || "",
    info.age || "",
    info.marital || "",
    info.job || "",
    info.coin || 0,
    info.score || 0,
    info.mainTimer || "00:00"
  ]);
}

/**
 * ذخیره اطلاعات بازی‌ها
 */
function saveGamesInfo(sheet, data) {
  const sheetName = "GamesInfo";
  let ws = sheet.getSheetByName(sheetName);
  if (!ws) {
    ws = sheet.insertSheet(sheetName);
    ws.appendRow([
      "Timestamp",
      "UserName",
      "GameNumber",
      "GameTimer",
      "MiniGameTimers",
      "Options"
    ]);
  }
  
  const info = data.infoAccount;
  const games = data.gamesInfo || [];
  
  for (let i = 0; i < games.length; i++) {
    const game = games[i];
    ws.appendRow([
      new Date(),
      info.userName || "",
      i + 1,
      game.gameTimer || "00:00",
      (game.minigameTimer || []).join(" | "),
      (game.options || []).join(" | ")
    ]);
  }
}

/**
 * ذخیره نتایج پرسشنامه‌ها
 */
function saveQuiz(sheet, data) {
  const quizs = data.quizs || [];
  
  for (let q = 0; q < quizs.length; q++) {
    const sheetName = "Quiz" + (q + 1);
    let ws = sheet.getSheetByName(sheetName);
    if (!ws) {
      ws = sheet.insertSheet(sheetName);
      const headers = ["Timestamp", "UserName", "Timer"];
      const quizData = quizs[q].quiz || [];
      for (let i = 0; i < quizData.length; i++) {
        headers.push("Q" + (i + 1));
      }
      ws.appendRow(headers);
    }
    
    const row = [
      new Date(),
      data.infoAccount.userName || "",
      quizs[q].timer || "00:00"
    ];
    
    const quizData = quizs[q].quiz || [];
    for (let i = 0; i < quizData.length; i++) {
      row.push(quizData[i]);
    }
    
    ws.appendRow(row);
  }
}

/**
 * ساخت پاسخ JSON
 */
function createResponse(status, message) {
  return ContentService.createTextOutput(JSON.stringify({
    status: status,
    message: message
  })).setMimeType(ContentService.MimeType.JSON);
}