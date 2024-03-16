using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private SkateSkinInfo[] _skateSkinInfo;

    private void Awake()
    {
        ShopManager.OnSelectSkin += SetSkinTexture;
    }
    private void OnDestroy()
    {
        ShopManager.OnSelectSkin -= SetSkinTexture;
    }
    private void Start()
    {
        Invoke(nameof(LoadSkin), 1f); 
    }
    private void LoadSkin()
    {
        for (int i = 0; i < _skateSkinInfo.Length; i++)
            _skateSkinInfo[i].Load();
    }
    private void SetSkinTexture(ItemType itemType, SkateSkinsInfo skinInfo)
    {
        for (int i = 0; i < _skateSkinInfo.Length; i++)
            _skateSkinInfo[i].SetSkinTexture(itemType, skinInfo);
    }
    [System.Serializable]
    private struct SkateSkinInfo
    {
        [SerializeField] private Material mat;
        [SerializeField] private ItemType item_type;
        [Space(5)]
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Material _lineRenderMat;

        public void Load()
        {
            mat.mainTexture = GameDataManager.SkateTexture;

            int r = Random.Range(0, 2); 
            if(r == 1)
                _lineRenderer.material = _lineRenderMat;
        }
        public void SetSkinTexture(ItemType itemType, SkateSkinsInfo skinInfo)
        {
            if (itemType != item_type) return;

            mat.mainTexture = skinInfo.Texture;
            _lineRenderer.material = _lineRenderMat;
        }
    }
}


