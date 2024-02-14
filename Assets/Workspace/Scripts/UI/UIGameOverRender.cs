using System.Collections;
using TMPro;
using UnityEngine;

public class UIGameOverRender : MonoBehaviour
{
    [SerializeField] private Transform _finishPanel;
    [SerializeField] private TextMeshProUGUI _finishScoreText;
    [SerializeField] private TextMeshProUGUI _finishCoinsText;
    [SerializeField] private TextMeshProUGUI _finishKeyText;
    [Space(10)]
    [SerializeField] private Transform _losePanel;
    [SerializeField] private TextMeshProUGUI _loseScoreText;

    private const float delay_value = 1.0f; 

    private void OnEnable()
    {
        GameManager.onFinish += RenderFinishPanel;
        GameManager.onLose += RenderLosePanel; 
    }
    private void OnDisable()
    {
        GameManager.onFinish -= RenderFinishPanel;
        GameManager.onLose -= RenderLosePanel;
    }
    private void RenderFinishPanel() => StartCoroutine(RenderDaley(true));
    private void RenderLosePanel() => StartCoroutine(RenderDaley(false));
    private IEnumerator RenderDaley(bool isFinish)
    {
        yield return new WaitForSeconds(delay_value);

        if (isFinish)
        {
            _finishPanel.gameObject.SetActive(true);
            _finishScoreText.text = $"Best Score: {GameDataManager.BestLevelDistance[GameDataManager.CurrentLevel]}";
            _finishCoinsText.text = $"{CoinsManager.currentCoinsCount}";
            _finishKeyText.text = $"{KeyManager.currentKeysCount}";
        }
        else
        {
            _losePanel.gameObject.SetActive(true);
            _loseScoreText.text = $"Score: {DistanceMaanger.Distance}";
        }
    }
}
