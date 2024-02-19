using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class LevelReview : MonoBehaviour
{
    [SerializeField] private Sprite _goldSprite;
    [SerializeField] private Sprite _silverSprite;
    [SerializeField] private Sprite _bronzeSprite;
    [Space(5)]
    [SerializeField] private UIMoveTween _moveTween; 

    private Image _image;
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
        ServiceLocator.GetService<UIGameOverRender>().onPanelRenderEnded += Show; 
    }

    private void Show()
    {
        if (GetSprite() == null) return;

        _image.enabled = true;
        _image.sprite = GetSprite();    
        _moveTween.Show(); 
    }

    private Sprite GetSprite()
    {
        LevelTimeInfo levelTimeInfo = ServiceLocator.GetService<LevelManager>().ActiveLevel.LevelTimeInfo;
        float gameTime = TimerManager.gameTime;

        if (gameTime <= levelTimeInfo.goldTime) 
            return _goldSprite;

        if (gameTime <= levelTimeInfo.silverTime)
            return _silverSprite;

        if (gameTime <= levelTimeInfo.bronzeTime)
            return _bronzeSprite;

        return null; 
    }
    private void OnDestroy()
    {
        ServiceLocator.GetService<UIGameOverRender>().onPanelRenderEnded -= Show;
    }
}
