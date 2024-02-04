using TMPro;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIScoreRender : MonoBehaviour
{
    [SerializeField] private bool _isStatic;

    [SerializeField] private Color _scoreColor;
    [SerializeField] private Color _loseColor;


    private TextMeshProUGUI _text; 
    private const float tween_duration = .5f;
    private const float text_lifeTIme = 1.5f;

    private float _currentTextLifeTime;

    private void OnEnable()
    {
        if (!_isStatic)
            UIScoreRenderController.RegistAddedScoreUIRender(this);
        else
            UIScoreRenderController.RegistStaticScoreUIRender(this); 
    }
    private void Update()
    {
        if (_isStatic) return; 

        if (_currentTextLifeTime <= 0)
            RenderTxt("");

        _currentTextLifeTime -= Time.deltaTime;
    }
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    Color GetTextColor(bool lose) => !lose ? _scoreColor : _loseColor;
    public void RenderTxt(string text, bool lose = false, Vector3 pos = new Vector3())
    { 
        _text.text = text;
        _currentTextLifeTime = text_lifeTIme;


        if (!_isStatic)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos);
            _text.rectTransform.DOPunchScale(Vector3.one * tween_duration, tween_duration);
            _text.DOColor(GetTextColor(lose), tween_duration / 2f);
            _text.rectTransform.DOMove(screenPosition, tween_duration / 2f)
                .SetEase(Ease.Flash);
        }
    }
}

