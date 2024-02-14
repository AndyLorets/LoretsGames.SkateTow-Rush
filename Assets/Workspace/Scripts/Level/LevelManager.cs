using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform _levelsParent; 

    private Level[] _levels;
    private int _currentLevel; 

    private void Awake()
    {
        GameManager.onNextLevel += NextLevel;
        GameManager.onSaveAll += Save;
        GameManager.onRestart += OnRestart; 
    }
    private void Start()
    {
        Construt();
    }
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
    }
}
