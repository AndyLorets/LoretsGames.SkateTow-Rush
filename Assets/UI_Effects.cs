using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Effects : MonoBehaviour
{
    [SerializeField] private SpeedLineUIEffect _speedLineUIEffect; 

    private Player _player;

    void Start()
    {
        _player = ServiceLocator.GetService<Player>();
        _speedLineUIEffect.Construct(); 
    }
    private void Update()
    {
        if(_player.isBoost && !_speedLineUIEffect.active)
            _speedLineUIEffect.Enable();
        if (!_player.isBoost && _speedLineUIEffect.active)
            _speedLineUIEffect.Disable();
    }
}

[System.Serializable]
public class SpeedLineUIEffect
{
    [SerializeField] private Image _speedlinesImage;

    private RectTransform _speedlinesRect;
    private Tween _speedlinesImageTween;
    private Tween _speedlinesRectTween;

    private const float speedlines_fade_max_value = 0.6f;
    private const float speedlines_fade_min_value = 0.3f;
    private const float speedlines_scale_max_value = 1.6f;
    private const float speedlines_scale_min_value = 1f;
    private const float tween_duration = .3f;

    public bool active { get; private set; }

    public void Construct()
    {
        _speedlinesRect = _speedlinesImage.GetComponent<RectTransform>();
        active = _speedlinesImage.enabled; 
        Disable(); 
    }
    public void Enable()
    {
        if (active) return;

        active = true;
        TweenSpeedlinesImage(active);
        TweenSpeedlinesRect(active);

        ServiceLocator.GetService<CameraManager>().PostEffectTweening(); 
    }
    public void Disable()
    {
        if (!active) return;

        active = false; 
        TweenSpeedlinesImage(active);
        TweenSpeedlinesRect(active);

        ServiceLocator.GetService<CameraManager>().PostEffectTweening();
    }
    private void TweenSpeedlinesImage(bool state)
    {
        _speedlinesImageTween?.Kill();

        if (!state)
        {
            _speedlinesImage.DOColor(Color.clear, tween_duration)
                .OnComplete(() => _speedlinesImage.enabled = state);
            return;
        }
        else if (!_speedlinesImage.enabled)
        {
            _speedlinesImage.enabled = active;
            _speedlinesImage.color = Color.clear;
        }

        float r = Random.Range(speedlines_fade_min_value, speedlines_fade_max_value);

        _speedlinesImageTween = _speedlinesImage.DOColor(new Color(1, 1, 1, r), tween_duration)
            .OnComplete(() => TweenSpeedlinesImage(state));
    }
    private void TweenSpeedlinesRect(bool state)
    {
        _speedlinesRectTween?.Kill();

        if (!state)
        {
            _speedlinesRect.transform.localScale = Vector3.one;
            return;
        }

        float r = Random.Range(speedlines_scale_min_value, speedlines_scale_max_value);

        _speedlinesRectTween = _speedlinesRect.DOScale(Vector3.one * r, tween_duration)
            .OnComplete(() => TweenSpeedlinesRect(state));
    }
}
