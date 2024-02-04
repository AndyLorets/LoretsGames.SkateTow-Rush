using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const int framerate_value = 60;

    public static Action onFinish;
    public static Action onLose;
    public static Action onNextLevel;
    public static Action onRestart;

    public static int nextLevel_fade_duration = 1; 
    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {
        StartCoroutine(SetFrameRate(framerate_value));

        CoinsManager.Init();
        UIScoreRenderController.Init();
    }
    private void Initialize()
    {
        ServiceLocator.RegisterService(this);
    }
    public static void Finish() => onFinish?.Invoke();
    public void Lose() => onLose?.Invoke();
    public void NextLevel() => onNextLevel?.Invoke();
    public void Restart() => onRestart?.Invoke();
    private IEnumerator SetFrameRate(int frameRate)
    {
        QualitySettings.vSyncCount = 0;
        while (true)
        {
            if (frameRate != Application.targetFrameRate)//000100087
                Application.targetFrameRate = frameRate;
            yield return null;
        }
    }
    private void OnDestroy()
    {
        ScoreManager.Reset();
        CoinsManager.Reset(); 
        UIScoreRenderController.Reset();
        UICoinsRenderController.Reset();
    }
}
