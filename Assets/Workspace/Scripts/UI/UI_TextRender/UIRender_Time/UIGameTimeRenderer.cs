using UnityEngine;

public class UIGameTimeRenderer : UIRendererBase
{
    [SerializeField] private UIMoveTween _uiMoveTween;
    protected override void Regist()
    {
        _isStatic = true;   
        UITimerRenderController.RegistStaticUIRender(this);
    }
    protected override void Awake()
    {
        base.Awake();

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
}
