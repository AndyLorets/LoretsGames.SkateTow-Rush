using System;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private SkinInfo[] _skinOnfo;
    private static int _currentSkinLevel;

    private void Awake()
    {
        ShopManager.onSoldItem += SetSkinLevel;
    }
    private void Start()
    {
        Construct(); 
    }
    private void OnDestroy()
    {
        ShopManager.onSoldItem -= SetSkinLevel;
    }
    private void SetSkinLevel(ItemType itemType, int value, int maxValue)
    {
        for (int i = 0; i < _skinOnfo.Length; i++)
            _skinOnfo[i].SetSkinLevel(itemType, value, maxValue);

        Construct();
    }
    private void Construct()
    {
        _currentSkinLevel = GameDataManager.ItemValue.GetValue(_skinOnfo[_currentSkinLevel].itemKey);

        for (int i = 0; i < _skinOnfo[_currentSkinLevel].ActiveObj.Length; i++)
        {
            _skinOnfo[_currentSkinLevel].ActiveObj[i].SetActive(true);
        }
        for (int i = 0; i < _skinOnfo[_currentSkinLevel].DeactiveObj.Length; i++)
        {
            _skinOnfo[_currentSkinLevel].DeactiveObj[i].SetActive(false);
        }
    }
    [Serializable]
    private struct SkinInfo
    {
        [field: SerializeField] public GameObject[] ActiveObj { get; private set; }
        [field: SerializeField] public GameObject[] DeactiveObj { get; private set; }
        [field: SerializeField] public ItemType item_type { get; private set;}
        public string itemKey => ItemConvertor.ConvertTitleFromType(item_type);

        public void SetSkinLevel(ItemType itemType, int value, int maxValue)
        {
            if (itemType != item_type) return;

            int lastValue = GameDataManager.ItemValue.GetValue(itemKey);
            int endValue = Math.Clamp(lastValue + value, lastValue + value, maxValue);
            GameDataManager.ItemValue.SetValue(itemKey, endValue);
        }
    }

}


