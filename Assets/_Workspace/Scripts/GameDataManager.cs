using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public static class GameDataManager
{
    private static readonly string _savePath;

    private const string SAVE_KEY = "SavesJSON";
    public const int LEVELS_COUNT = 15;

    public static Action onFirstSave;

    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        public List<TKey> keys = new List<TKey>();
        public List<TValue> values = new List<TValue>();

        public SerializableDictionary() { }

        public SerializableDictionary(Dictionary<TKey, TValue> dict)
        {
            foreach (var kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }
        public bool ContainsKey(TKey key)
        {
            return keys.Contains(key);
        }
        // Получение значения по ключу
        public TValue GetValue(TKey key)
        {
            int index = keys.IndexOf(key);
            if (index >= 0 && index < values.Count)
            {
                return values[index];
            }
            throw new KeyNotFoundException($"Key not found: {key}");
        }

        // Установка значения по ключу
        public void SetValue(TKey key, TValue value)
        {
            int index = keys.IndexOf(key);
            if (index >= 0)
            {
                values[index] = value;
            }
            else
            {
                keys.Add(key);
                values.Add(value);
            }
        }
        public Dictionary<TKey, TValue> ToDictionary()
        {
            var dict = new Dictionary<TKey, TValue>();
            for (int i = 0; i < keys.Count; i++)
            {
                dict.Add(keys[i], values[i]);
            }
            return dict;
        }
    }
    private class SaveDataContainer
    {
        public int currentLevel;
        public int moneyCount;
        public int gemCount;
        public SerializableDictionary<int, float> bestLevelTime = new SerializableDictionary<int, float>();
        public SerializableDictionary<string, int> itemValue = new SerializableDictionary<string, int>();
        public SerializableDictionary<string, int> itemPrice = new SerializableDictionary<string, int>();
    }

    private static SaveDataContainer _saveDataContainer = new SaveDataContainer();

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
    public static int GemCount
    {
        get => _saveDataContainer.gemCount;
        set => _saveDataContainer.gemCount = value;
    }
    public static SerializableDictionary<int, float> BestLevelTime
    {
        get => _saveDataContainer.bestLevelTime;
        set => _saveDataContainer.bestLevelTime = value;
    }
    public static SerializableDictionary<string, int> ItemValue
    {
        get => _saveDataContainer.itemValue;
        set => _saveDataContainer.itemValue = value;
    }
    public static SerializableDictionary<string, int> ItemPrice
    {
        get => _saveDataContainer.itemPrice;
        set => _saveDataContainer.itemPrice = value;
    }
    static GameDataManager()
    {
#if UNITY_EDITOR
        string saveFolder = Path.Combine("_Workspace", "Saves");
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
    private static void FirstSave()
    {
        onFirstSave?.Invoke();
        Save();
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
}
