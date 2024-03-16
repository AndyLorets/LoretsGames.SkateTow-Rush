using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public abstract class UItemPrefabBase : MonoBehaviour
{
    [Space(5)]
    [SerializeField] protected TextMeshProUGUI _titelText;
    [SerializeField] protected TextMeshProUGUI _pricelText;
    [SerializeField] protected Image _priceIcon;

    protected Button _buyBtn;
    private Color _successColor;
    private Color _failedColor;

    private const float tween_duration = .5f;
    private const string success_hex_color = "#0CFF00";
    private const string failed_hex_color = "#FF3600";

    protected virtual void Awake()
    {
        _buyBtn = GetComponent<Button>();
    }
    private void Start()
    {
        Invoke(nameof(Construct), 1f);
    }
    protected virtual void Construct()
    {
        ColorUtility.TryParseHtmlString(success_hex_color, out _successColor);
        ColorUtility.TryParseHtmlString(failed_hex_color, out _failedColor);
    }

    public abstract void Button();
    protected virtual void OnSuccess()
    {
        GameDataManager.Save();
        Construct();

        TweenColor(_successColor);
        TweenPunchScale(.2f);
    }
    protected virtual void OnFailed()
    {
        TweenColor(_failedColor);
        TweenPunchScale(.15f, 5);
    }
    private void TweenColor(Color color)
    {
        Color startColor = _pricelText.color;
        _pricelText.DOKill();
        _pricelText.DOColor(color, tween_duration / 2)
            .OnComplete(() => _pricelText.DOColor(startColor, tween_duration / 2));
    }
    private void TweenPunchScale(float scale, int vibrato = 1)
    {
        transform.DORewind(); 
        transform.DOKill();
        transform.DOPunchScale(Vector3.one * scale, tween_duration, vibrato);
    }
}
