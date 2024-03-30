using DG.Tweening;
using UnityEngine;
public class UITimerRenderer : UIRendererBase
{
    [SerializeField] private Color _onChangeRedColor = Color.red;
    private Color _startColor = Color.white;

    private UIMoveTween _uiMoveTween; 

    private const float tween_duretion = .5f;

    private Tween _scaleTween;
    private Tween _colorTween;
    protected override void Regist()
    {
        if (!_isStatic)
            UITimerRenderController.RegistAddedUIRender(this);
        else
            UITimerRenderController.RegistStaticUIRender(this);
    }
    protected override void Awake()
    {
        base.Awake();

        _uiMoveTween = GetComponent<UIMoveTween>();
        _startColor = _text.color;

        if (_isStatic)
        {
            GameManager.onGameStart += Show;
            GameManager.onFinish += Hide;
            GameManager.onLose += Hide;
        }
        else _text.enabled = false; 
    }
    private void OnDestroy()
    {
        if (_isStatic)
        {
            GameManager.onGameStart -= Show;
            GameManager.onFinish -= Hide;
            GameManager.onLose -= Hide;
        }
    }
    private void Hide() => _uiMoveTween.Hide();
    private void Show() => _uiMoveTween.Show();
    public override void RenderTxt(string text, Vector3 startPos = default)
    {
        if (_isStatic)
        {
            base.RenderTxt(text, startPos);

            if (TimerManager.gameDeadLineTime <= 5)
            {
                AnimateScale();
                AnimateColor();
            }
            return;
        }
        _text.enabled = true;   
        _text.text = text;

        base.RenderTxt(text, startPos);
    }
    protected override void DeactiveAdded()
    {
        base.DeactiveAdded();
        _text.enabled = false;
    }
    private void AnimateScale()
    {
        if (_scaleTween != null)
            if (_scaleTween.IsPlaying()) return;  

        _scaleTween = _text.transform.DOScale(Vector3.one * 1.25f, tween_duretion)
            .OnComplete(() => _scaleTween = _text.transform.DOScale(Vector3.one, tween_duretion)); 
    }
    private void AnimateColor()
    {
        if (_colorTween != null)
            if (_colorTween.IsPlaying()) return;

        _colorTween = _text.DOColor(_onChangeRedColor, tween_duretion)
            .OnComplete(() => _colorTween = _text.DOColor(_startColor, tween_duretion));
    }
}
