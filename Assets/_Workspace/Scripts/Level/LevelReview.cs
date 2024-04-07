using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LevelReview : MonoBehaviour
{
    [SerializeField] private Sprite _enableStar;
    [SerializeField] private Sprite _dissableStar;
    [Space(5)]
    [SerializeField] private Image[] _starImages;


    private void Start()
    {
        Initialize();
    }
    private void Initialize()   
    {
        ServiceLocator.GetService<UIGameOverRender>().onPanelRenderEnded += Show;
        for (int i = 0; i < _starImages.Length; i++)
            _starImages[i].sprite = _dissableStar;
    }

    private void Show()
    {
        if (GetStarCount() == 0) return;

        StartCoroutine(ShowStar());
    }
    private IEnumerator ShowStar()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        int index = 0;
        while (index < GetStarCount())
        {
            _starImages[index].transform.DOKill(); 
            _starImages[index].sprite = _enableStar;
            _starImages[index].transform.DOPunchScale(Vector3.one * .5f, 1, 1); 
            index++; 
            yield return waitForSeconds;
        }
    }
    private int GetStarCount()
    {
        LevelTimeInfo levelTimeInfo = ServiceLocator.GetService<LevelManager>().ActiveLevel.LevelTimeInfo;
        float gameTime = Time.time;

        if (gameTime <= levelTimeInfo.goldTime) 
            return 3;

        if (gameTime <= levelTimeInfo.silverTime)
            return 2;

        if (gameTime <= levelTimeInfo.bronzeTime)
            return 1;

        return 0; 
    }
    private void OnDestroy()
    {
        ServiceLocator.GetService<UIGameOverRender>().onPanelRenderEnded -= Show;
    }
}
