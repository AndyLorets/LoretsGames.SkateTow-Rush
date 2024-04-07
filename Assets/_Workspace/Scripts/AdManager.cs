using GamePush;
using UnityEngine;

public static class AdManager 
{
    private static float _interNextShowTime; 

    public const string REWARD_100 = "REWARD_100";
    public const string RESPAWN = "RESPAWN";
    private const float INTER_COOLDOWN = 60;

#if UNITY_EDITOR
    public static bool RewardedAvailable = true;
#elif UNITY_WEBGL
    public static bool RewardedAvailable => GP_Ads.IsRewardedAvailable(); 
#endif
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
