using UnityEngine;

public static class ScoreManager
{
    public static System.Action<int, Vector3> onAddScore;
    private static int _score;
    public static int Score => _score;

    public static void AddScore(int score, Vector3 pos)
    {
        _score += score;
        onAddScore?.Invoke(score, pos); 
    }
    public static void Reset() => _score = 0;
}
