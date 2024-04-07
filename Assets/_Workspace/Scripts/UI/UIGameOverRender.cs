using DG.Tweening;
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
    [SerializeField] private UIMoveTween _winButtonsTween;
    [Space(10)]
    [SerializeField] private Image _darkPanel;


    private Color _darkColor;

    public Action onPanelRenderEnded; 

    private const float delay_value = 1.0f; 

    private void OnEnable()
    {
        GameManager.onFinish += RenderFinishPanel;
    }
    private void OnDisable()
    {
        GameManager.onFinish -= RenderFinishPanel;
    }
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        _darkColor = _darkPanel.color;
        _darkPanel.color = Color.clear;
        _darkPanel.enabled = false; 

    }
    string TimeString => TextTranslator.CurrentTextLanguage("Time", "Время"); 
    private void RenderFinishPanel() => StartCoroutine(RenderDaley(true));
    private IEnumerator RenderDaley(bool isFinish)
    {
        _darkPanel.enabled = true; 
        _darkPanel.DOColor(_darkColor, delay_value);
        UIMoveTween uIMoveTween = null; 

        yield return new WaitForSeconds(delay_value); 

        if (isFinish)
        {
            int money = MoneyManager.currentCount;
            int keys = GemManager.currentGemCount;

            uIMoveTween = _winButtonsTween;
            _finishPanel.gameObject.SetActive(true);
            _finishTimeText.text = $"{TimeString}: {UIGameTimeRenderer.GameTimeText()}";
            _finishMoneyText.text = $"{money}";
            _finishKeyText.text = $"{keys}";
        }

        yield return new WaitForSeconds(delay_value);

        uIMoveTween?.Show(); 
        onPanelRenderEnded?.Invoke(); 
    }
    
}
