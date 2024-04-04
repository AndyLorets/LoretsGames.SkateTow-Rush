using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class UIDistanceMap : MonoBehaviour
{
    [SerializeField] private Image _fillAmount;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private UIMoveTween _uiMoveTween;

    private Transform _player;
    private Transform _finish; 

    private void Awake()
    {
        GameManager.onGameStart += OnGameStart;
        GameManager.onFinish += OnGameOver;
        GameManager.onLose += OnGameOver;
    }
    private void OnDestroy()
    {
        GameManager.onGameStart -= OnGameStart;
        GameManager.onFinish -= OnGameOver;
        GameManager.onLose -= OnGameOver;
    }
    private void Start()
    {
        Invoke(nameof(Construct), 1f); 
    }
    private void Construct()
    {
        _player = ServiceLocator.GetService<Player>().transform;
        _finish = ServiceLocator.GetService<LevelManager>().FinishTransform;

        string text = TextTranslator.CurrentTextLanguage("LEVEL", "спнбемэ"); 
        _levelText.text = $"{text}: {GameDataManager.CurrentLevel + 1}";
    }
    private void OnGameStart()
    {
        _uiMoveTween.Show();
        float totalDist = Vector3.Distance(_player.position, _finish.position);
        StartCoroutine(RenderUI(totalDist)); 
    }
    private void OnGameOver()
    {
        _uiMoveTween.Hide(); 
    }
    private IEnumerator RenderUI(float totalDist)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        while (!GameManager.isGameOver)
        {
            float currentDist = Vector3.Distance(_player.position, _finish.position);
            float progress = 1 - (currentDist / totalDist);
            _fillAmount.fillAmount = progress;

            yield return waitForSeconds;
        }
 
    }
}
