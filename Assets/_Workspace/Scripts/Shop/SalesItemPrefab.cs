using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class SalesItemPrefab : MonoBehaviour
{
    [SerializeField] private SalesItemInfo _item;
    [Space(5)]
    [SerializeField] private Image _art;
    [SerializeField] private Image _priceIcon;
    [SerializeField] private TextMeshProUGUI _pricelText;
    [SerializeField] private TextMeshProUGUI _currentValueText;
    [SerializeField] private TextMeshProUGUI _titelText;
   
    private Button _buyBtn; 
    private Color _successColor; 
    private Color _failedColor;

    private const float tween_duration = .5f;
    private const string success_hex_color = "#0CFF00";
    private const string failed_hex_color = "#FF3600";

    private void Awake()
    {
        _buyBtn = GetComponent<Button>();
        _item = Instantiate(_item);
        _item.Init(); 
    }
    private void Start()
    {
        Construct(); 
    }
    private void Construct()
    {
        _item.Load(); 

        _titelText.text = ItemConvertor.ConvertTitleFromType(_item.Type);
        _pricelText.text = _item.Price.ToString();
        _art.sprite = _item.Art;
        _priceIcon.sprite = _item.PriceIcon;
        _currentValueText.text = _item.Value.ToString(); 

        ColorUtility.TryParseHtmlString(success_hex_color, out _successColor);
        ColorUtility.TryParseHtmlString(failed_hex_color, out _failedColor);

        if (_item.Value >= _item.MaxValue)
            _buyBtn.interactable = false;
    }

    public void BuyButton()
    {
        if (_item.Value < _item.MaxValue)
            ShopManager.Sell(_item, OnSuccess, OnFailed);
        else _buyBtn.interactable = false; 
    }
    private void OnSuccess()
    {
        _item.UpdateInfo(); 
        GameDataManager.Save();
        Construct();

        TweenColor(_successColor);
        TweenPunchScale(.2f);
        Debug.Log($"Sell <color=yellow>{ItemConvertor.ConvertTitleFromType(_item.Type)}</color> <color=#0CFF00>Success!</color>");
    }
    private void OnFailed()
    {
        TweenColor(_failedColor);
        TweenPunchScale(.15f, 5); 
        Debug.Log($"Sell <color=yellow>{ItemConvertor.ConvertTitleFromType(_item.Type)}</color> <color=#FF3600>Failed!</color>!");
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
        transform.DOKill(); 
        transform.DOPunchScale(Vector3.one * scale, tween_duration, vibrato); 
    }
}
