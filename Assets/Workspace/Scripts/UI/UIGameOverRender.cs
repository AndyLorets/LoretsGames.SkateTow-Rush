using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIGameOverRender : MonoBehaviour
{
    [SerializeField] private Transform _finishPanel;
    [SerializeField] private TextMeshProUGUI _finishTimeText;
    [SerializeField] private TextMeshProUGUI _finishMoneyText;
    [SerializeField] private TextMeshProUGUI _finishKeyText;
    [Space(10)]
    [SerializeField] private Transform _losePanel;
    [SerializeField] private TextMeshProUGUI _loseScoreText;

    private Image _darkImage; 
    public Action onPanelRenderEnded; 

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
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        _darkImage = GetComponent<Image>();
        _darkImage.enabled = false;
    }
    private void RenderFinishPanel() => StartCoroutine(RenderDaley(true));
    private void RenderLosePanel() => StartCoroutine(RenderDaley(false));
    private IEnumerator RenderDaley(bool isFinish)
    {
        yield return new WaitForSeconds(delay_value);

        _darkImage.enabled = true; 

        if (isFinish)
        {
            float time = TimerManager.gameTime;
            int money = MoneyManager.currentCount;
            int keys = KeyManager.currentKeysCount;

            _finishPanel.gameObject.SetActive(true);
            _finishTimeText.text = $"Time: {UITimerRenderController.GameTimeText()}";
            _finishMoneyText.text = $"{money}";
            _finishKeyText.text = $"{keys}";
        }
        else
        {
            _losePanel.gameObject.SetActive(true);
            _loseScoreText.text = $"Time: {UITimerRenderController.GameTimeText()}";
        }

        yield return new WaitForSeconds(delay_value);

        onPanelRenderEnded?.Invoke(); 
    }
}
