using DG.Tweening;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(UIMoveTween))]
public class UITextTimerRenderer : UITextRendererBase
{
    [SerializeField] private Color _onChangeRedColor = Color.red;
    [SerializeField] private Color _onChangeBlueColor = Color.green;
    private Color _startColor = Color.white;

    private UIMoveTween _uiMoveTween; 

    private const float tween_duretion = .25f;

    private Tween _scaleTween;
    private Tween _colorTween;
    protected override void Regist()
    {
        if (!_isStatic)
            UITextTimerRenderController.RegistAddedUIRender(this);
        else
            UITextTimerRenderController.RegistStaticUIRender(this);
    }
    protected override void Awake()
    {
        base.Awake();   

        _uiMoveTween = GetComponent<UIMoveTween>();   
        _startColor = _text.color;

        GameManager.onGameStarted += Show;
        GameManager.onFinish += Hide;
        GameManager.onLose += Hide;
    }
    private void OnDestroy()
    {
        GameManager.onGameStarted -= Show;
        GameManager.onFinish -= Hide;
        GameManager.onLose -= Hide;
    }
    private void Hide() => _uiMoveTween.Hide();
    private void Show() => _uiMoveTween.Show();
    public override void RenderTxt(string text, Vector3 startPos = default)
    {
        if(!_isStatic) return;  

        base.RenderTxt(text, startPos);
        int timer = TimerManager.gameTimer;

        AnimateScale();

        if (timer <= 5)
            AnimateColor(_onChangeRedColor);
    }
    private void AnimateScale()
    {
        _scaleTween.Kill(); 
        _scaleTween = _text.transform.DOScale(Vector3.one * 1.1f, tween_duretion)
            .OnComplete(() => _scaleTween = _text.transform.DOScale(Vector3.one, tween_duretion)); 
    }
    private void AnimateColor(Color color)
    {
        _colorTween.Kill();
        _colorTween = _text.DOColor(color, tween_duretion)
            .OnComplete(() => _colorTween = _text.DOColor(_startColor, tween_duretion));
    }
}
