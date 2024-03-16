using GamePush;
using UnityEngine;

public static class AdManager 
{
    public const string MONEY_250_REWARD = "MONEY_250_REWARD"; 

    public static void ShowFullScreen()
    {
        GP_Ads.ShowFullscreen(OnFullscreenStart, OnFullscreenClose);
    }
    public static void ShowReward(string rewardName)
    {
        GP_Ads.ShowRewarded(rewardName, OnRewardedReward, OnRewardedStart, OnRewardedClose);
    }
    private static void OnRewardedReward(string rewardName)
    {
        switch(rewardName)
        {
            case MONEY_250_REWARD: 
                MoneyManager.Add(250); 
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
