using UnityEngine;
public static class CoinsManager 
{
    public static int currentCoinsCount { get; private set; }
    public static System.Action<int, Vector3> onChangeCoins;

    public static void Init()
    {
        GameManager.onSaveAll += Save;
    }
    public static void AddCoins(int value, Vector3 startPos = new Vector3())
    {
        currentCoinsCount += value;
        onChangeCoins?.Invoke(value, startPos);  
    }
    public static void RemoveCoins(int value)
    {
        GameDataManager.CoinsCount -= value;
        onChangeCoins?.Invoke(value, new Vector3());
    }
    public static void Save()
    {
        GameDataManager.CoinsCount += currentCoinsCount;
    }
    public static void Reset()
    {
        GameManager.onSaveAll -= Save;
        currentCoinsCount = 0;
    }
}
