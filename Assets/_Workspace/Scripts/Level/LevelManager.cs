using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform _levelsParent;
    public Transform FinishTransform { get; set; } 

    private Level[] _levels;
    private int _currentLevel;

    private void Awake()
    {
        ServiceLocator.RegisterService(this); 

        GameManager.onNextLevel += NextLevel;
        GameManager.onSaveAll += Save;
        GameManager.onRestart += OnRestart;
        GameDataManager.onFirstSave += OnFirstSave;
 
        Invoke(nameof(Construt), .2f);
    }
    public Level ActiveLevel => _levels[_currentLevel]; 

    private void Construt()
    {
        _currentLevel = GameDataManager.CurrentLevel;

        Init();

        if (_levels.Length != GameDataManager.LEVELS_COUNT)
        {
            if (GameManager.DEBBUG_WARNINGLOG)
                Debug.LogWarning("Number of levels does not match the constant in <color=green>GameDataLoader</color>\"");
        }
    }
    private void Init()
    {
        if (_levels == null)
            _levels = new Level[_levelsParent.childCount];

        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i] = _levelsParent.GetChild(i).GetComponent<Level>();
            _levels[i].gameObject.SetActive(false); 
        }

        _levels[_currentLevel].gameObject.SetActive(true);
    }

    private void NextLevel()
    {
        if (_currentLevel < _levels.Length - 1)
            _currentLevel++;
        else _currentLevel = 0;
    }
    private void OnFirstSave()
    {
        if(_levels == null)
            _levels = new Level[_levelsParent.childCount];

        for (int i = 0; i < _levels.Length; i++)
        {
            GameDataManager.BestLevelTime.SetValue(i, 0);
        }
    }
    private void Save()
    {
        GameDataManager.CurrentLevel = _currentLevel;
    }

    private void OnRestart()
    {
    }
    private void OnDestroy()
    {
        GameManager.onNextLevel -= NextLevel;
        GameManager.onSaveAll -= Save; 
        GameManager.onRestart -= OnRestart;
        GameDataManager.onFirstSave -= OnFirstSave;
    }
}
