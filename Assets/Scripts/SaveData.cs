using UnityEngine;

public static class SaveData
{
    private const string current_level_prefs = "current_level_prefs"; 
    public static int CurrentLevel
    {                                    
        get => PlayerPrefs.GetInt(current_level_prefs, 0);
        set => PlayerPrefs.SetInt(current_level_prefs, value); 
    }

    private const string coins_prefs = "Coins";
    public static int CoinsCount
    {
        get => PlayerPrefs.GetInt(coins_prefs, 0);
        set => PlayerPrefs.SetInt(coins_prefs, value);
    }
}