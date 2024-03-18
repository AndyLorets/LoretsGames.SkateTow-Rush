using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIUpgradePrefab : UItemPrefabBase
{
    [SerializeField] private UpgradeInfo _item;
    [SerializeField] private Image _art;
    [SerializeField] private TextMeshProUGUI _currentValueText;
    protected override void Awake()
    {
        base.Awake();

        _item = Instantiate(_item);
        _item.Init();
    }
    protected override void Construct()
    {
        base.Construct();

        _item.Load();

        _art.sprite = _item.Art;

        _titelText.text = ItemConvertor.ConvertTitleFromType(_item.Type);
        _pricelText.text = _item.Price.ToString();

        _priceIcon.sprite = _item.PriceIcon;
        _currentValueText.text = $"{_item.UserValue} lvl";

        if (_item.StartValue >= _item.MaxValue)
            _buyBtn.interactable = false;
    }
    public override void Button()
    {
        if (_item.StartValue < _item.MaxValue)
            ShopManager.Upgrade(_item, OnSuccess, OnFailed, _item.IncrimentValue, _item.MaxValue);
        else _buyBtn.interactable = false;

        AudioManager.PlayOneShot(AudioManager.SoundType.Click); 
    }
    protected override void OnSuccess()
    {
        _item.UpdateInfo();

        base.OnSuccess();

        Debug.Log($"Sell <color=yellow>{ItemConvertor.ConvertTitleFromType(_item.Type)}</color> <color=#0CFF00>Success!</color>");
    }
    protected override void OnFailed()
    {
        base.OnFailed();

        Debug.Log($"Sell <color=yellow>{ItemConvertor.ConvertTitleFromType(_item.Type)}</color> <color=#FF3600>Failed!</color>!");
    }
    private void OnDestroy()
    {
        _item.Reset();  
    }
}
