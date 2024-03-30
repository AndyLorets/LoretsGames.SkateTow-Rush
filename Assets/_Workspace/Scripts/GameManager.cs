using GamePush;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIMoveTween[] _hideOnStartUIMoveTweens;
    [SerializeField] private UIMoveTween _pauseTween;
    [SerializeField] private UIMoveTween _pauseBtnTween;
    [SerializeField] private Image _pauseBG; 
    [SerializeField] private Button _upgradeButton;     

    public static Action onFinish;
    public static Action onLose;
    public static Action onNextLevel;
    public static Action onRestart;
    public static Action onGameStart;
    public static Action onSaveAll;
    public static bool isGameOver { get; private set; }
    public static bool isGameStart { get; private set; }
    public static bool showKeyPicked { get; set; }

    public static bool DEBBUG_LOG = true;
    public static bool DEBBUG_WARNINGLOG = true;
    public static bool DEBBUG_ERRORLOG = false;

    public const int nextLevel_fade_duration = 1;
    private const int framerate_value = 60;
    private void Awake()
    {
        SetDebbugState();

        if(FindObjectOfType(typeof(SceneLoader)) == null)
            Debug.LogWarning("Missing <color=green>SceneLoader</color> component! Try running the game from the <color=black>Load_Scene</color>.");

        ServiceLocator.RegisterService(this);
        onGameStart += HideTweens;
    }
    private void OnEnable()
    {
        GP_Game.OnPause += Pause;
        GP_Game.OnResume += Resume;
    }
    private void OnDisable()
    {
        GP_Game.OnPause -= Pause;
        GP_Game.OnResume -= Resume;
    }
    private void Start()
    {
        InitializeAll();
        StartCoroutine(SetFrameRate(framerate_value));
        Invoke(nameof(ShowUpgrades), .5f); 
    }
    public void Pause()
    {
        Time.timeScale = 0; 
        AudioListener.pause = true;
        _pauseBG.enabled = true;
        _pauseTween.Show();
        _pauseBtnTween.Hide(); 
    }
    public void Resume()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        _pauseBG.enabled = false;
        _pauseTween.Hide();
        _pauseBtnTween.Show(); 
    }
    private void ShowUpgrades()
    {
        if (GameDataManager.CurrentLevel == 0) return; 

        _upgradeButton.onClick.Invoke(); 
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
    private void HideTweens()
    {
        for (int i = 0; i < _hideOnStartUIMoveTweens.Length; i++)
        {
            _hideOnStartUIMoveTweens[i].Hide();
        }
    }
    public void StartGame()
    {
        if (isGameStart || isGameOver) 
            return; 

        onGameStart?.Invoke();
        isGameStart = true;

        _pauseBtnTween.Show();

        StartCoroutine(TimerManager.GameTimer());

        CameraManager.ChangeCam(CameraManager.cam_game_name);

        if (DEBBUG_LOG)
            Debug.Log($"Game is Started. <color=#00FFFF>Current Level: {GameDataManager.CurrentLevel}</color>\"");
    }
    private void InitializeAll()
    {
        // Load statics class
        GameDataManager.Load();
        MoneyManager.Init();
        GemManager.Init();
        TimerManager.Init();
        UIMoneyRenderController.Init();
        UIGemRenderController.Init();
        UITimerRenderController.Init();

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

        AdManager.ShowFullScreen(); 
    }
    public void Restart()
    {
        onRestart?.Invoke();

        if (DEBBUG_LOG)
            Debug.Log($"Game is Restarted. <color=#00FFFF>Current Level: {GameDataManager.CurrentLevel}</color>\"");

        AdManager.ShowFullScreen();
    }
    public static void SaveAll()
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
        showKeyPicked = false;

        // ResetAll statics class
        MoneyManager.Reset();    
        GemManager.Reset();
        TimerManager.Reset();
        UIMoneyRenderController.Reset();
        UIGemRenderController.Reset();
        UITimerRenderController.Reset();

        onGameStart -= HideTweens;

        if (DEBBUG_LOG)
            Debug.Log("Reset All"); 
    }
    private void OnDestroy() => ResetAll();
}
