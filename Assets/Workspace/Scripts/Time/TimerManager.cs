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
    public static void Init()
    {
        GameManager.onSaveAll += Save;
        GameManager.onFinish += GetFinishedLevel;
    }
    public static void Init(int value)
    {
        gameDeadLineTime += value;
    }
    public static void AddTime(int value)
    {
        gameDeadLineTime += value;
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
        float lastBestTime = GameDataManager.BestLevelTime[_finishedLevel];
        if (gameTime < lastBestTime)
            GameDataManager.BestLevelTime[_finishedLevel] = gameTime;
    }
    public static void Reset()
    {
        gameDeadLineTime = 0;
        gameTime = 0; 
        GameManager.onSaveAll -= Save;
        GameManager.onFinish -= GetFinishedLevel;
    }
}
