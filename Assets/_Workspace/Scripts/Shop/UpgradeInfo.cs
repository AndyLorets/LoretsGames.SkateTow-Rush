using UnityEngine;
[CreateAssetMenu(fileName = "New_UpgradeInfo", menuName = "ScriptableObjects/UpgradeInfo")]
public class UpgradeInfo : ItemInfoBase
{
    [field: SerializeField] public Sprite Art { get; private set; }
    [field: SerializeField] public int StartValue { get; private set; }
    [field: SerializeField] public int MaxValue { get; private set; }
    [field: SerializeField] public int IncrimentValue { get; private set; } = 1;
    [field: SerializeField] public int IncrimentPrice { get; private set; } = 1;
    public int UserValue => StartValue - GameDataManager.UpgradeValue.GetValue(nameof(StartValue)) + 1; 

    public void Init()
    {
        GameDataManager.onFirstSave += OnFirstSave;
    }
    public void Load()
    {
        if (GameDataManager.UpgradePrice.ContainsKey(ItemConvertor.ConvertTitleFromType(Type)))
        {
            Price = GameDataManager.UpgradePrice.GetValue(ItemConvertor.ConvertTitleFromType(Type));
            StartValue = GameDataManager.UpgradeValue.GetValue(ItemConvertor.ConvertTitleFromType(Type));
        }
    }
    private void OnFirstSave()
    {
        if (!GameDataManager.UpgradePrice.ContainsKey(ItemConvertor.ConvertTitleFromType(Type)))
        {
            GameDataManager.UpgradePrice.SetValue(ItemConvertor.ConvertTitleFromType(Type), Price);
            GameDataManager.UpgradeValue.SetValue(ItemConvertor.ConvertTitleFromType(Type), StartValue);
            GameDataManager.UpgradeValue.SetValue(nameof(StartValue), StartValue);
        }
    }
    public void UpdateInfo()
    {
        GameDataManager.UpgradePrice.SetValue(ItemConvertor.ConvertTitleFromType(Type), (int)(Price + IncrimentPrice));
        Price = GameDataManager.UpgradePrice.GetValue(ItemConvertor.ConvertTitleFromType(Type));
        StartValue = GameDataManager.UpgradeValue.GetValue(ItemConvertor.ConvertTitleFromType(Type));
    }
    public void Reset()
    {
        GameDataManager.onFirstSave -= OnFirstSave;
    }
}