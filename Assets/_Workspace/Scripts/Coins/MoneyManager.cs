using UnityEngine;
public static class MoneyManager 
{
    public static int currentCount { get; private set; }
    public static System.Action<int, Vector3> onChange;

    public static void Init()
    {
        GameManager.onSaveAll += Save;
    }
    public static void Add(int value, Vector3 startPos = new Vector3())
    {
        currentCount += value;
        onChange?.Invoke(value, startPos);
        AudioManager.PlayOneShot(AudioManager.SoundType.Picked); 
    }
    public static void Remove(int value)
    {
        GameDataManager.MoneyCount -= value;
        onChange?.Invoke(value, new Vector3());
    }
    public static void Save()
    {
        GameDataManager.MoneyCount += currentCount;
    }
    public static void Reset()
    {
        GameManager.onSaveAll -= Save;
        currentCount = 0;
    }
}
