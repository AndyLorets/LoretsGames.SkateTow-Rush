using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAds : MonoBehaviour
{
    [SerializeField] private Image _bg;
    [SerializeField] private UIMoveTween _panel;

    private Color _showColor; 
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        _showColor = _bg.color;
        _bg.color = Color.clear;
        _bg.enabled = false;
    }
    public void Show()
    {
        _panel.Show();
        _bg.enabled = true; 
        _bg.DOColor(_showColor, .5f); 
    }
    public void Hide()
    {
        _panel.Hide();
        _bg.DOColor(Color.clear, 1f).OnComplete(() => _bg.enabled = false);
    }
    public void ShowRewardAd()
    {
        AdManager.ShowReward(AdManager.REWARD_100);
        Hide(); 
    }
}
