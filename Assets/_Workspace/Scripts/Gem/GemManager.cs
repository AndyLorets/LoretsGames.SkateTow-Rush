using UnityEngine;

public static class GemManager 
{
    public static System.Action<int, Vector3> onChangeValue;
    public static int currentGemCount { get; private set; }
    public static void Init()
    {
        GameManager.onSaveAll += Save;
    }
    public static void AddGem(int value, Vector3 startPos)
    {
        currentGemCount += value;
        onChangeValue?.Invoke(value, startPos);
    }
    public static void RemoveGem(int value)
    {
        GameDataManager.GemCount -= value;
        onChangeValue?.Invoke(value, Vector3.zero);
    }
    public static void Save()
    {
        GameDataManager.GemCount += currentGemCount;
    }
    public static void Reset()
    {
        GameManager.onSaveAll -= Save;
        currentGemCount = 0;
    }
}
