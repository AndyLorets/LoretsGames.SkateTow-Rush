using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform _levelsParent; 

    private Level[] _levels;

    private void Awake()
    {
        GameManager.onNextLevel += OnNextLevel;
        GameManager.onRestart += OnRestart; 
        Construt();
    }
    private void Construt()
    {
        _levels = new Level[_levelsParent.childCount];

        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i] = _levelsParent.GetChild(i).GetComponent<Level>();
            _levels[i].gameObject.SetActive(false); 
        }

        _levels[SaveData.CurrentLevel].gameObject.SetActive(true);    
    }

    private void OnNextLevel()
    {
        if (SaveData.CurrentLevel < _levels.Length - 1)
            SaveData.CurrentLevel++;
        else SaveData.CurrentLevel = 0;
    }
    private void OnRestart()
    {
    }
    private void OnDestroy()
    {
        GameManager.onRestart -= OnRestart;
        GameManager.onNextLevel -= OnNextLevel;
    }
}
