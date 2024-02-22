using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform _levelsParent; 

    private Level[] _levels;
    private int _currentLevel; 

    private void Awake()
    {
        ServiceLocator.RegisterService(this); 

        GameManager.onNextLevel += NextLevel;
        GameManager.onSaveAll += Save;
        GameManager.onRestart += OnRestart;
        GameDataManager.onFirstSave += OnFirstSave;

        Construt();
    }
    public Level ActiveLevel => _levels[_currentLevel]; 
    private void Construt()
    {
        _levels = new Level[_levelsParent.childCount];
        _currentLevel = GameDataManager.CurrentLevel; 
        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i] = _levelsParent.GetChild(i).GetComponent<Level>();
            _levels[i].gameObject.SetActive(false); 
        }

        _levels[_currentLevel].gameObject.SetActive(true); 
        
        if (_levels.Length != GameDataManager.LEVELS_COUNT)
        {
            if (GameManager.DEBBUG_WARNINGLOG)
                Debug.LogWarning("Number of levels does not match the constant in <color=green>GameDataLoader</color>\""); 
        }
    }

    private void NextLevel()
    {
        if (_currentLevel < _levels.Length - 1)
            _currentLevel++;
        else _currentLevel = 0;
    }
    private void OnFirstSave()
    {
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
