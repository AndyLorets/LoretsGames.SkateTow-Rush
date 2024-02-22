using System;
public static class ShopManager 
{
    public static Action<ItemType, int, int> onSoldItem; 
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
    private static void SetSellRes(PriceType priceType, int value)
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
    public static void Sell(SalesItemInfo salesItemInfo, Action onSuccess, Action onFailed)
    {
        int price = salesItemInfo.Price;
        int value = salesItemInfo.IncrimentValue;
        int maxValue = salesItemInfo.MaxValue;
        PriceType priceType = salesItemInfo.PriceType;
        ItemType itemType = salesItemInfo.Type;

        if (GetSellRes(priceType) < price)
        {
            onFailed?.Invoke(); 
            return;
        }

        onSoldItem?.Invoke(itemType, value, maxValue);
        onSuccess?.Invoke(); 

        SetSellRes(priceType, price);
    }
}
