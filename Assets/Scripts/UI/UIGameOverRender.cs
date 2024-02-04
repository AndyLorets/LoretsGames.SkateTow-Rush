using System.Collections;
using TMPro;
using UnityEngine;

public class UIGameOverRender : MonoBehaviour
{
    [SerializeField] private Transform _finishPanel;
    [SerializeField] private TextMeshProUGUI _finishScoreText;
    [SerializeField] private TextMeshProUGUI _finishCoinsText;
    [Space(10)]
    [SerializeField] private Transform _losePanel;
    [SerializeField] private TextMeshProUGUI _loseScoreText;

    private const float delay_value = 1.0f; 

    private void OnEnable()
    {
        GameManager.onFinish += OnFinish;
        GameManager.onLose += OnLose; 
    }
    private void OnDisable()
    {
        GameManager.onFinish -= OnFinish;
        GameManager.onLose -= OnLose;
    }
    private void OnFinish()
    {
        if (!gameObject.activeSelf) 
            gameObject.SetActive(true); 
        Render(true);
    }
    private void OnLose()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        Render(false);
    }
    private void Render(bool isFinish) => StartCoroutine(RenderDaley(isFinish)); 
    private IEnumerator RenderDaley(bool isFinish)
    {
        yield return new WaitForSeconds(delay_value);

        if (isFinish)
        {
            _finishPanel.gameObject.SetActive(true);
            _finishScoreText.text = $"Score: {ScoreManager.Score}";
            _finishCoinsText.text = $"{CoinsManager.currentCoinsCount}"; 
        }
        else
        {
            _losePanel.gameObject.SetActive(true);
            _loseScoreText.text = $"Score: {ScoreManager.Score}";
        }
    }
}
