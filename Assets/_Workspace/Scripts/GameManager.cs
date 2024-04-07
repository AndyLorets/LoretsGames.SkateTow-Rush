using GamePush;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIMoveTween[] _hideOnStartUIMoveTweens;
    [SerializeField] private UIMoveTween _pauseTween;
    [SerializeField] private UIMoveTween _pauseBtnTween;
    [SerializeField] private Image _pauseBG; 
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private UIAds _respawnAds;

    public static Action onFinish;
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

    private static GameManager _instance; 

    private void Awake()
    {
        if (_instance == null)
            _instance = this; 

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

        AdManager.ShowFullScreen();
    }
    public static void Pause()
    {
        Time.timeScale = 0; 
        AudioListener.pause = true;

        if (_instance == null) return;

        _instance._pauseBG.enabled = true;
        _instance._pauseTween.Show();
        _instance._pauseBtnTween.Hide(); 
    }
    public static void Resume()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;

        if (_instance == null) return;

        _instance._pauseBG.enabled = false;
        _instance._pauseTween.Hide();
        _instance._pauseBtnTween.Show(); 
    }
    private void ShowUpgrades()
    {
        if (GameDataManager.MoneyCount < 100) return; 

        _upgradeButton.onClick.Invoke(); 
    }
    private void SetDebbugState(bool state = false)
    {
#if UNITY_EDITOR
        return;
#else
    DEBBUG_LOG = state;
    DEBBUG_WARNINGLOG = state;
    DEBBUG_ERRORLOG = state;
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

        isGameStart = true;
        onGameStart?.Invoke();

        _pauseBtnTween.Show();

        CameraManager.ChangeCam(CameraManager.cam_game_name);

        if (DEBBUG_LOG)
            Debug.Log($"<color=#00FFFF>Game is Started</color>\"");
    }
    private void InitializeAll()
    {
        // Load statics class
        GameDataManager.Load();
        MoneyManager.Init();
        GemManager.Init();
        UIMoneyRenderController.Init();
        UIGemRenderController.Init();

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
            Debug.Log($"<color=#00FFFF>Game is Finished.</color>\"");
    }
    public static void Lose()
    {
        if (isGameOver) return;

        if (AdManager.RewardedAvailable)
            _instance._respawnAds.Show();
        else
            Finish();
 
        if (DEBBUG_LOG)
            Debug.Log($"<color=#00FFFF>Game is Losed.</color>\"");
    }
    public void Restart()
    {
        Resume(); 
        onRestart?.Invoke();
        SaveAll();

        if (DEBBUG_LOG)
            Debug.Log($"<color=#00FFFF>Game is Restarted.</color>\"");
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
        UIMoneyRenderController.Reset();
        UIGemRenderController.Reset();

        onGameStart -= HideTweens;

        if (DEBBUG_LOG)
            Debug.Log("Reset All"); 
    }
    private void OnDestroy() => ResetAll();
}
