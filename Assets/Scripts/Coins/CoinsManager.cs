using UnityEngine;
public static class CoinsManager 
{
    public static int currentCoinsCount { get; private set; }  

    public static void Init()
    {
        UICoinsRenderController.RenderStaticCoins($"{SaveData.CoinsCount}");
        GameManager.onFinish += Save;
    }

    public static void AddCoins(int value, Vector3 startPos)
    {
        currentCoinsCount += value;
        RenderText(value, startPos);
    }
    public static void RemoveCoins(int value)
    {
        currentCoinsCount -= value;
        RenderText(value, Vector3.zero);
    }
    private static void RenderText(float value, Vector3 startPos)
    {
        string textAddedCoins = $"+{value}$";
        string textStaticCoins = $"{currentCoinsCount + SaveData.CoinsCount}";
        UICoinsRenderController.RenderAddedCoins(textAddedCoins, startPos);
        UICoinsRenderController.RenderStaticCoins(textStaticCoins);
    }
    public static void Save()
    {
        SaveData.CoinsCount += currentCoinsCount;
    }
    public static void Reset()
    {
        GameManager.onFinish -= Save;
        currentCoinsCount = 0;
    }
}
