using UnityEngine;

[CreateAssetMenu(fileName = "SkateSkinsInfo_", menuName = "ScriptableObjects/SkateSkinsInfo")]
public class SkateSkinsInfo : ItemInfoBase
{
    [field: SerializeField] public Texture Texture { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public bool Avable { get; private set; } = false;
    private string _itemKey => ItemConvertor.ConvertTitleFromType(Type);

    public void Init()
    {
        GameDataManager.onFirstSave += OnFirstSave;
    }
    public void Reset()
    {
        GameDataManager.onFirstSave -= OnFirstSave;
    }
    public void Load()
    {
        Avable = GameDataManager.SkinAvable.GetValue(_itemKey); 
    }
    public void Select()
    {
        Avable = true;

        GameDataManager.SkateTexture = Texture;
        GameDataManager.SkinAvable.SetValue(_itemKey, Avable);
    }
    private void OnFirstSave()
    {
        if (!GameDataManager.SkinAvable.ContainsKey(ItemConvertor.ConvertTitleFromType(Type)))
        {
            GameDataManager.SkinAvable.SetValue(ItemConvertor.ConvertTitleFromType(Type), Avable);
        }

        if (Avable)
            GameDataManager.SkateTexture = Texture;
    }
}
