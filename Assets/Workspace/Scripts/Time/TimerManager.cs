using UnityEngine;
using System.Collections;
using System;

public static class TimerManager
{
    public static Action<bool> onTimeChanged;
    public static int gameTimer { get; private set; }
    public static void Init()
    {
    }
    public static void AddTime(int value)
    {
        gameTimer += value;
        onTimeChanged?.Invoke(true);
    }
    public static IEnumerator GameTimer()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        while (GameManager.isGameStart)
        {
            gameTimer -= 1;

            if (gameTimer <= 0)
                GameManager.Lose();

            onTimeChanged?.Invoke(false);

            yield return waitForSeconds;
        }
    }
    public static void Reset()
    {
        gameTimer = 0;
    }
}
