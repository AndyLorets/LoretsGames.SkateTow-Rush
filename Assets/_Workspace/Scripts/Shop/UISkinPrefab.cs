using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkinPrefab : UItemPrefabBase
{
    [SerializeField] private TextMeshProUGUI _stateText; 
    [SerializeField] private Image _art;
    [SerializeField] private SkateSkinsInfo _item;
    bool _selected => GameDataManager.SkateTexture == _item.Texture; 

    protected override void Awake()
    {
        base.Awake();

        _item = Instantiate(_item);
        _item.Init();

        ShopManager.OnSelectSkin += OnOtherSkinSelect; 
    }
    private void OnDestroy()
    {
        _item.Reset();
        ShopManager.OnSelectSkin -= OnOtherSkinSelect;
    }
    private void OnOtherSkinSelect(ItemType itemType, SkateSkinsInfo skateSkinsInfo)
    {
        if (_selected) return;

        Construct(); 
    }
    protected override void Construct()
    {
        base.Construct();

        _item.Load(); 

        _art.sprite = _item.Sprite;

        _titelText.text = ItemConvertor.ConvertTitleFromType(_item.Type);

        _pricelText.enabled = !_item.Avable; 
        _pricelText.text = _item.Price.ToString();

        _priceIcon.enabled = !_item.Avable; 
        _priceIcon.sprite = _item.PriceIcon;

        string Choosed = TextTranslator.CurrentTextLanguage("Choosed", "Выбрано");
        string Select = TextTranslator.CurrentTextLanguage("Select", "Выбрать");

        _stateText.enabled = _selected || _item.Avable;
        _stateText.text = _selected ? Choosed : Select;
        _stateText.color = _selected ? Color.green : Color.white; 
    }
    public override void Button()
    {
        ShopManager.SetSkin(_item, OnSuccess, OnFailed);
        AudioManager.PlayOneShot(AudioManager.SoundType.Click); 
    }
    protected override void OnSuccess()
    {
        _item.Select();

        base.OnSuccess();

        Debug.Log($"Sell <color=yellow>{ItemConvertor.ConvertTitleFromType(_item.Type)}</color> <color=#0CFF00>Success!</color>");
    }
    protected override void OnFailed()
    {
        base.OnFailed();

        ServiceLocator.GetService<UIAds>().Show(); 
        Debug.Log($"Sell <color=yellow>{ItemConvertor.ConvertTitleFromType(_item.Type)}</color> <color=#FF3600>Failed!</color>!");
    }
}
