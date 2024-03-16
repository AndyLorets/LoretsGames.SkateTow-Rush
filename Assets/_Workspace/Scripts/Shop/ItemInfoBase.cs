using UnityEngine;

public abstract class ItemInfoBase : ScriptableObject
{
    [field: SerializeField] public ItemType Type { get; protected set; }
    [field: SerializeField] public PriceType PriceType { get; protected set; }
    [field: SerializeField] public int Price { get; protected set; }
    [field: SerializeField] public Sprite PriceIcon { get; protected set; }

}

public enum PriceType
{
    Money, Gem
}
public enum ItemType
{
    UpgradeMoveSpeed, UpgradeTime, 
    SkinSkate_1, SkinSkate_2, SkinSkate_3, SkinSkate_4, SkinSkate_5
}
public struct ItemConvertor
{
    public static string ConvertTitleFromType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.UpgradeMoveSpeed: return $"Upgrade Move Speed";
            case ItemType.UpgradeTime: return $"Upgrade Time";
            case ItemType.SkinSkate_1: return $"Skate Skin 1";
            case ItemType.SkinSkate_2: return $"Skate Skin 2";
            case ItemType.SkinSkate_3: return $"Skate Skin 3";
            case ItemType.SkinSkate_4: return $"Skate Skin 4";
            case ItemType.SkinSkate_5: return $"Skate Skin 5";
        }
        return "";
    }
}
