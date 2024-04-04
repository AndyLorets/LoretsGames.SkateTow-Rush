using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using GamePush;
public static class GameDataManager
{
    private static readonly string _savePath;

    private const string SAVE_KEY = "SavesJSON";
    public const int LEVELS_COUNT = 15;

    public static Action onFirstSave;

    private static SaveDataContainer _saveDataContainer = new SaveDataContainer();

    #region SerializableDictionary

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
    #endregion

    private class SaveDataContainer
    {
        public int currentLevel;
        public int moneyCount;
        public int gemCount;
        public SerializableDictionary<int, float> bestLevelTime = new SerializableDictionary<int, float>();
        public SerializableDictionary<string, int> upgradeValue = new SerializableDictionary<string, int>();
        public SerializableDictionary<string, int> upgradePrice = new SerializableDictionary<string, int>();

        public SerializableDictionary<string, bool> skinAvable = new SerializableDictionary<string, bool>();
        public Texture skateTexture;
    }

    // GENERAL
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
    // UPGRADE
    public static SerializableDictionary<string, int> UpgradeValue
    {
        get => _saveDataContainer.upgradeValue;
        set => _saveDataContainer.upgradeValue = value;
    }
    public static SerializableDictionary<string, int> UpgradePrice
    {
        get => _saveDataContainer.upgradePrice;
        set => _saveDataContainer.upgradePrice = value;
    }
    // SKINS
    public static SerializableDictionary<string, bool> SkinAvable
    {
        get => _saveDataContainer.skinAvable;
        set => _saveDataContainer.skinAvable = value;
    }
    public static Texture SkateTexture
    {
        get => _saveDataContainer.skateTexture;
        set => _saveDataContainer.skateTexture = value;
    }
    static GameDataManager()
    {
#if UNITY_EDITOR 
        string saveFolder = Path.Combine("_Workspace", $"Saves");
        _savePath = Path.Combine(Application.dataPath, saveFolder, $"{SAVE_KEY}.json");
#elif !UNITY_WEBGL
        _savePath = Path.Combine(Application.streamingAssetsPath, $"{SAVE_KEY}.json");       
#endif

#if !UNITY_WEBGL || UNITY_EDITOR
        string directoryPath = Path.GetDirectoryName(_savePath);
        if (!string.IsNullOrEmpty(directoryPath))
        {
            // Создать каталог, только если путь не пустой или null
            Directory.CreateDirectory(directoryPath);
        }
#endif
    }
    public static string GetJson()
    {
#if UNITY_EDITOR
        return File.ReadAllText(_savePath);
#elif UNITY_WEBGL
        return PlayerPrefs.GetString(SAVE_KEY);
#else
        return File.ReadAllText(_savePath);
#endif
    }
    public static void SetJson(string json)
    {
#if UNITY_EDITOR
        // Сохраняем JSON в файл
        File.WriteAllText(_savePath, json);
#elif UNITY_WEBGL
        PlayerPrefs.SetString(SAVE_KEY, json); 
#else
        // Сохраняем JSON в файл
        File.WriteAllText(_savePath, json);
#endif
    }
    public static void Load()
    {
        bool exists = false;

#if UNITY_EDITOR
        exists = File.Exists(_savePath);
#elif UNITY_WEBGL
        exists = PlayerPrefs.HasKey(SAVE_KEY);
#else
        exists = File.Exists(_savePath);
#endif

        if (exists)
        {
            try
            {
                // Десериализуем JSON в объект
                _saveDataContainer = JsonUtility.FromJson<SaveDataContainer>(GetJson());

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
        Load();
    }
    public static void Save()
    {
        // Сериализуем объект в JSON
        string json = JsonUtility.ToJson(_saveDataContainer);
        try
        {
            SetJson(json); 

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
