using GamePush;
using UnityEngine;

public static class AdManager 
{
    private static float _interNextShowTime; 

    public const string REWARD_100 = "REWARD_100";
    private const float INTER_COOLDOWN = 60; 
    public static void ShowFullScreen()
    {
        bool canShowInter = Time.time >= _interNextShowTime; 
        if (!canShowInter) return; 

        GP_Ads.ShowFullscreen(OnFullscreenStart, OnFullscreenClose);
        _interNextShowTime = Time.time + INTER_COOLDOWN;
    }
    public static void ShowReward(string rewardName)
    {
        GP_Ads.ShowRewarded(rewardName, OnRewardedReward, OnRewardedStart, OnRewardedClose);
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
        }
    }
    private static void OnRewardedStart()
    {
        Time.timeScale = 0;
    }
    private static void OnRewardedClose(bool succes)
    {
        Time.timeScale = 1; 
    }
    private static void OnFullscreenStart()
    {
        Time.timeScale = 0;
    }
    private static void OnFullscreenClose(bool succes)
    {
        Time.timeScale = 1;
    }
}
