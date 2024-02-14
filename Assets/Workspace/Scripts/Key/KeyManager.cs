using UnityEngine;

public static class KeyManager 
{
    public static System.Action<int, Vector3> onChangeKey;
    public static int currentKeysCount { get; private set; }
    public static void Init()
    {
        GameManager.onSaveAll += Save;
    }
    public static void AddKey(int value, Vector3 startPos)
    {
        currentKeysCount += value;
        onChangeKey?.Invoke(value, startPos);
    }
    public static void RemoveKey(int value)
    {
        GameDataManager.KeysCount -= value;
        onChangeKey?.Invoke(value, Vector3.zero);
    }
    public static void Save()
    {
        GameDataManager.KeysCount += currentKeysCount;
    }
    public static void Reset()
    {
        GameManager.onSaveAll -= Save;
        currentKeysCount = 0;
    }
}
