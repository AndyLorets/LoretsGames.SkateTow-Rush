using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action onFinish;
    public static Action onLose;
    public static Action onNextLevel;
    public static Action onRestart;
    public static Action onGameStarted;
    public static Action onSaveAll;
    public static bool isGameOver { get; private set; }
    public static bool isGameStart { get; private set; }

    public static bool DEBBUG_LOG = true;
    public static bool DEBBUG_WARNINGLOG = true;
    public static bool DEBBUG_ERRORLOG = false;

    public const int nextLevel_fade_duration = 1;
    private const int framerate_value = 60;

    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        GameDataManager.Load();
        SetDebbugState(); 
    }
    private void Start()
    {
        InitializeAll();
        StartCoroutine(SetFrameRate(framerate_value));
    }
    private void SetDebbugState()
    {
#if UNITY_EDITOR
        return;
#else
    DEBBUG_LOG = false;
    DEBBUG_WARNINGLOG = false;
    DEBBUG_ERRORLOG = false;
#endif
    }

    public static void StartGame()
    {
        onGameStarted?.Invoke();
        isGameStart = true;

        GameManager gameManager = ServiceLocator.GetService<GameManager>();
        gameManager?.StartCoroutine(TimerManager.GameTimer());

        CameraManager.ChangeCam(CameraManager.cam_game_name);

        if (DEBBUG_LOG)
            Debug.Log($"Game is Started. <color=#00FFFF>Current Level: {GameDataManager.CurrentLevel}</color>\"");
    }
    private void InitializeAll()
    {
        // Init statics class
        DistanceMaanger.Init();
        CoinsManager.Init();
        KeyManager.Init();
        UITextCoinsRenderController.Init();
        UITextTimerRenderController.Init();
        UITextKeyRenderController.Init();
        TimerManager.Init();

        if (DEBBUG_LOG)
            Debug.Log("Initialize All");
    }
    public static void Finish()
    {
        if (isGameOver) return;

        isGameOver = true;
        isGameStart = false;
        onFinish?.Invoke();

        if (DEBBUG_LOG)
            Debug.Log($"Game is Finished. <color=#00FFFF>Current Level: {GameDataManager.CurrentLevel}</color>\"");
    }
    public static void Lose()
    {
        if (isGameOver) return;

        isGameOver = true;
        isGameStart = false; 
        onLose?.Invoke();

        if (DEBBUG_LOG)
            Debug.Log($"Game is Losed. <color=#00FFFF>Current Level: {GameDataManager.CurrentLevel}</color>\"");
    }
    public void NextLevel()
    {
        onNextLevel?.Invoke();
        SaveAll();

        if (DEBBUG_LOG)
            Debug.Log($"Next Level. <color=#00FFFF>Current Level: {GameDataManager.CurrentLevel}</color>\"");
    }
    public void Restart()
    {
        onRestart?.Invoke();

        if (DEBBUG_LOG)
            Debug.Log($"Game is Restarted. <color=#00FFFF>Current Level: {GameDataManager.CurrentLevel}</color>\"");
    }
    private static void SaveAll()
    {
        onSaveAll?.Invoke();
        GameDataManager.Save();
    }
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
    private void ResetAll()
    {
        isGameOver = false;
        isGameStart = false;

        // ResetAll statics class
        DistanceMaanger.Reset();
        CoinsManager.Reset();    
        KeyManager.Reset();
        UITextCoinsRenderController.Reset();
        UITextTimerRenderController.Reset();
        UITextKeyRenderController.Reset();
        TimerManager.Reset();
        if (DEBBUG_LOG)
            Debug.Log("Reset All"); 
    }
    private void OnDestroy() => ResetAll();
}
