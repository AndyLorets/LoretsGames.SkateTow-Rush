using UnityEngine;

public static class DistanceMaanger
{
    public static int Distance { get; private set; }
    static int _finishedLevel;

    private static Vector3 _startPos;
    private static Vector3 _endPos; 
    public static void Init()
    {
        GameManager.onFinish += GetFinishedLevel;
        GameManager.onSaveAll += Save;
        TimerManager.onTimeChanged += SetDistance; 
        _startPos = ServiceLocator.GetService<Player>().transform.position; 
    }
    private static void GetFinishedLevel() => _finishedLevel = GameDataManager.CurrentLevel;
    private static void Save()
    {
        int lastBestDist = GameDataManager.BestLevelDistance[_finishedLevel];
        if (lastBestDist < Distance)
            GameDataManager.BestLevelDistance[_finishedLevel] = Distance;
    }
    private static void SetDistance(bool addTime)
    {
         Distance = (int)Vector3.Distance(_startPos, _endPos); 
    }
    public static void Reset()
    {
        Distance = 0;
        GameManager.onFinish -= GetFinishedLevel;
        GameManager.onSaveAll -= Save;
        TimerManager.onTimeChanged -= SetDistance;
    }
}
