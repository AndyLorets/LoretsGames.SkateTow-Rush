using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName ="New_SalesItemInfo", menuName = "ScriptableObjects/SalesItemInfo")]
public class SalesItemInfo : ScriptableObject
{
    [field: SerializeField] public int Price { get; private set; }
    [SerializeField] private float _coefficientPriceAfterSell;
    [field: SerializeField] public int IncrimentValue { get; private set; } = 1; 
    [field: SerializeField] public int Value { get; private set; }
    [field: SerializeField] public int MaxValue { get; private set; }
    [field: SerializeField] public Sprite Art { get; private set; }
    [field: SerializeField] public Sprite PriceIcon { get; private set; }
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public PriceType PriceType { get; private set; }

    public void Init()
    {
        GameDataManager.onFirstSave += OnFirstSave;
    }
    public void Load()
    {
        if (GameDataManager.ItemPrice.ContainsKey(ItemConvertor.ConvertTitleFromType(Type)))
        {
            Price = GameDataManager.ItemPrice.GetValue(ItemConvertor.ConvertTitleFromType(Type));
            Value = GameDataManager.ItemValue.GetValue(ItemConvertor.ConvertTitleFromType(Type));
        }
    }
    private void OnFirstSave()
    {
        if (!GameDataManager.ItemPrice.ContainsKey(ItemConvertor.ConvertTitleFromType(Type)))
        {
            GameDataManager.ItemPrice.SetValue(ItemConvertor.ConvertTitleFromType(Type), Price);
            GameDataManager.ItemValue.SetValue(ItemConvertor.ConvertTitleFromType(Type), Value);
        }
    }
    public void UpdateInfo()
    {
        GameDataManager.ItemPrice.SetValue(ItemConvertor.ConvertTitleFromType(Type), (int)(Price * _coefficientPriceAfterSell));
        Price = GameDataManager.ItemPrice.GetValue(ItemConvertor.ConvertTitleFromType(Type));
        Value = GameDataManager.ItemValue.GetValue(ItemConvertor.ConvertTitleFromType(Type)); 
    }
    public void Reset()
    {
        GameDataManager.onFirstSave -= OnFirstSave;
    }
}

public enum PriceType
{
    Money, Gem
}
public enum ItemType
{
    UpgradeMaxSpeed, UpgradeTime, BuySkinSkate_1, BuySkinSkate_2
}
public struct ItemConvertor
{
    private const string moveSpeedMax = "Move speed";
    private const string upgradeTime = "Added Time";
    private const string buy_skin_skate_1 = "Skin Skate 1";
    private const string buy_skin_skate_2 = "Skin Skate 2";
    public static string ConvertTitleFromType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.UpgradeMaxSpeed: return moveSpeedMax;
            case ItemType.UpgradeTime: return upgradeTime;
            case ItemType.BuySkinSkate_1: return buy_skin_skate_1;
            case ItemType.BuySkinSkate_2: return buy_skin_skate_2;
        }
        return "";
    }
}
