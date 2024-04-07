using GamePush;
using UnityEngine;
using System;
public static class AdManager 
{
    private static float _interNextShowTime = AD_COOLDOWN;
    private static float _rewardNextShowTime = AD_COOLDOWN;

    public const string REWARD_100 = "REWARD_100";
    public const string RESPAWN = "RESPAWN";
    private const float AD_COOLDOWN = 60;

#if UNITY_EDITOR
    public static bool RewardedAvailable => Time.time >= _rewardNextShowTime;
    public static bool FullScreenAvailable => Time.time >= _interNextShowTime;
#elif UNITY_WEBGL
    public static bool RewardedAvailable => GP_Ads.IsRewardedAvailable() && Time.time >= _rewardNextShowTime; 
    public static bool FullScreenAvailable => GP_Ads.IsFullscreenAvailable() && Time.time >= _interNextShowTime;
#endif

    public static void ShowFullScreen()
    {
        if (!FullScreenAvailable)
            return;

        GP_Ads.ShowFullscreen(OnFullscreenStart, OnFullscreenClose);
        _interNextShowTime = Time.time + AD_COOLDOWN;

        if (GameManager.DEBBUG_LOG)
            Debug.Log($"<color=#00FFFF>ShowFullScreen</color>\"");
    }
    public static void ShowReward(string rewardName)
    {
        if (!RewardedAvailable)
            return; 

        GP_Ads.ShowRewarded(rewardName, OnRewardedReward, OnRewardedStart, OnRewardedClose);
        _rewardNextShowTime = Time.time + AD_COOLDOWN;

        if (GameManager.DEBBUG_LOG)
            Debug.Log($"<color=#00FFFF>ShowReward</color>\"");
    }
    private static void OnRewardedReward(string rewardName)
    {
        switch(rewardName)
        {
            case REWARD_100: 
                MoneyManager.Add(100);
                GemManager.AddGem(100, Vector2.zero);
                GameManager.SaveAll(); 
                break;
            case RESPAWN:
                ServiceLocator.GetService<Player>().Respawn(); 
                break;
        }
    }
    private static void OnRewardedStart()
    {
        GameManager.Pause(); 
    }
    private static void OnRewardedClose(bool succes)
    {
        GameManager.Resume();
    }
    private static void OnFullscreenStart()
    {
        GameManager.Pause();
    }
    private static void OnFullscreenClose(bool succes)
    {
        GameManager.Resume(); 
    }
}
