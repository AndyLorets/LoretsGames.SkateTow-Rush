using UnityEngine;
using System.Collections;
using System;

public static class TimerManager
{
    public static Action<bool> onTimeChanged;
    private static int _finishedLevel;
    public static float gameTime { get; private set; }
    public static int gameDeadLineTime { get; private set; }

    private const float coroutine_update_time = .1f;
    private const ItemType item_type = ItemType.UpgradeTime; 

    private static int _addTimeValue; 

    public static void Init()
    {
        GameManager.onSaveAll += Save;
        GameManager.onFinish += GetFinishedLevel;
        ShopManager.OnUpgrade += SetAddTimeValue;
        _addTimeValue = GameDataManager.UpgradeValue.GetValue(ItemConvertor.ConvertTitleFromType(item_type)); 
    }
    public static void Init(int value)
    {
        gameDeadLineTime += value;
    }
    private static void SetAddTimeValue(ItemType itemType, int value, int maxValue)
    {
        if (itemType != item_type) return;

        string key = ItemConvertor.ConvertTitleFromType(item_type);
        int lastValue = GameDataManager.UpgradeValue.GetValue(key);
        int endValue = Math.Clamp(lastValue + value, lastValue + value, maxValue);
        GameDataManager.UpgradeValue.SetValue(key, endValue);
        _addTimeValue = GameDataManager.UpgradeValue.GetValue(key);
    }
    public static void AddTime()
    {
        gameDeadLineTime += _addTimeValue;
        onTimeChanged?.Invoke(true);
    }
    private static void GetFinishedLevel() => _finishedLevel = GameDataManager.CurrentLevel;
    public static IEnumerator GameTimer()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(coroutine_update_time);

        while (GameManager.isGameStart)
        {
            gameTime += coroutine_update_time;

            float number = gameTime;
            float integerPart = Mathf.Floor(number);

            if (Mathf.Abs(number - integerPart) < coroutine_update_time)
                gameDeadLineTime -= 1;

            if (gameDeadLineTime <= 0)
                GameManager.Lose();

            onTimeChanged?.Invoke(false);

            yield return waitForSeconds;
        }
    }
    private static void Save()
    {
        float lastBestTime = GameDataManager.BestLevelTime.GetValue(_finishedLevel);
        if (gameTime < lastBestTime || lastBestTime == 0)
        {
            GameDataManager.BestLevelTime.SetValue(_finishedLevel, gameTime);
        }
    }
    public static void Reset()
    {
        gameDeadLineTime = 0;
        gameTime = 0; 
        GameManager.onSaveAll -= Save;
        GameManager.onFinish -= GetFinishedLevel;
        ShopManager.OnUpgrade -= SetAddTimeValue;
    }
}
