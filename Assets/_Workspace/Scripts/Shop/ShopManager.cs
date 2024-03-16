using System;

public static class ShopManager 
{
    public static Action<ItemType, int, int> OnUpgrade;
    public static Action<ItemType, SkateSkinsInfo> OnSelectSkin;
    private static int GetSellRes(PriceType priceType)
    {
        switch (priceType)
        {
            case PriceType.Money:
                return GameDataManager.MoneyCount;
            case PriceType.Gem:
                return GameDataManager.GemCount;
        }
        return 0;   
    }
    private static void RemoveRes(PriceType priceType, int value)
    {
        switch (priceType)
        {
            case PriceType.Money:
                MoneyManager.Remove(value);
                break; 
            case PriceType.Gem:
                GemManager.RemoveGem(value);
                break;
        }
    }
    public static void SetSkin(SkateSkinsInfo skinInfo, Action onSuccess, Action onFailed)
    {
        int price = skinInfo.Price;
        PriceType priceType = skinInfo.PriceType;
        ItemType itemType = skinInfo.Type;

        if (!skinInfo.Avable)
        {
            if (GetSellRes(priceType) < price)
            {
                onFailed?.Invoke();
                return;
            }

            RemoveRes(priceType, price);
        }

        onSuccess?.Invoke();
        OnSelectSkin?.Invoke(itemType, skinInfo);
    }
    public static void Upgrade(ItemInfoBase salesItemInfo, Action onSuccess, Action onFailed, int incrimentValue, int maxValue)
    {
        int price = salesItemInfo.Price;
        PriceType priceType = salesItemInfo.PriceType;
        ItemType itemType = salesItemInfo.Type;

        if (GetSellRes(priceType) < price)
        {
            onFailed?.Invoke();
            return;
        }

        OnUpgrade?.Invoke(itemType, incrimentValue, maxValue);
        onSuccess?.Invoke();

        RemoveRes(priceType, price);
    }
}
