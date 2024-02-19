using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class GameDataManager
{
    private static readonly string _savePath;

    private const string SAVE_KEY = "SavesJSON";
    public const int LEVELS_COUNT = 15; 
        
    private static SaveDataContainer _saveDataContainer = new SaveDataContainer();

    private class SaveDataContainer
    {
        public int currentLevel;
        public int moneyCount;
        public int keysCount;
        public int hookMoveSpeedMax;
        public List<float> bestLevelTime;
    }
    public static int CurrentLevel
    {
        get => _saveDataContainer.currentLevel;
        set => _saveDataContainer.currentLevel = value;
    }
    public static int MoneyCount
    {
        get => _saveDataContainer.moneyCount;
        set => _saveDataContainer.moneyCount = value;
    }
    public static int KeysCount
    {
        get => _saveDataContainer.keysCount;
        set => _saveDataContainer.keysCount = value;
    }
    public static int OnHookMoveMaxSpeed
    {
        get => _saveDataContainer.hookMoveSpeedMax;
        set => _saveDataContainer.hookMoveSpeedMax = value;
    }
    public static List<float> BestLevelTime
    {
        get => _saveDataContainer.bestLevelTime;
        set => _saveDataContainer.bestLevelTime = value; 
    } 

    static  GameDataManager()
    {
#if UNITY_EDITOR
        string saveFolder = Path.Combine("Workspace", "Saves");
        _savePath = Path.Combine(Application.dataPath, saveFolder, $"{SAVE_KEY}.json");
#else
        _savePath = Path.Combine(Application.persistentDataPath, $"{SAVE_KEY}.json");
#endif
        // Создаем директорию сохранения, если она отсутствует
        string directoryPath = Path.GetDirectoryName(_savePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public static void Load()
    {
        // Проверяем наличие файла перед загрузкой
        if (File.Exists(_savePath))
        {
            try
            {
                // Читаем JSON из файла
                string json = File.ReadAllText(_savePath);
                // Десериализуем JSON в объект
                _saveDataContainer = JsonUtility.FromJson<SaveDataContainer>(json);

                if (GameManager.DEBBUG_LOG)
                    Debug.Log("Data loaded");
            }
            catch (IOException e)
            {
                if (GameManager.DEBBUG_ERRORLOG)
                    Debug.LogError($"Failed to load data: {e.Message}");
            }
        }
        else
        {
            FirstSave(); 

            if (GameManager.DEBBUG_WARNINGLOG)
                Debug.LogWarning("Save file not found. Creating a new one.");
        }
    }

    public static void Save()
    {
        // Сериализуем объект в JSON
        string json = JsonUtility.ToJson(_saveDataContainer);
        try
        {
            // Сохраняем JSON в файл
            File.WriteAllText(_savePath, json);
            if (GameManager.DEBBUG_LOG)
                Debug.Log($"Data saved");
        }
        catch (IOException e)
        {
            if (GameManager.DEBBUG_ERRORLOG)
                Debug.LogError($"Failed to save data: {e.Message}");
        }
    }

    private static void FirstSave()
    {
        _saveDataContainer.bestLevelTime = new List<float>(); 
        for (int i = 0; i < LEVELS_COUNT; i++)
        {
            _saveDataContainer.bestLevelTime.Add(0);
        }

        Save();
    }
}
