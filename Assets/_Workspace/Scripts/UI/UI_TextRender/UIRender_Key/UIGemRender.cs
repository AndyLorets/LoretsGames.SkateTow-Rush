using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UIGemRender : UIRendererBase
{
    private Image _image;
    protected override void Awake()
    {
        if (_isStatic)
        {
            base.Awake();
            return;
        }

        _image = GetComponent<Image>();
        _image.enabled = false;

        base.Awake();
    }
    protected override void Regist()
    {
        if (!_isStatic)
            UIGemRenderController.RegistAddedUIRender(this);
        else
            UIGemRenderController.RegistStaticUIRender(this);
    }
    public override void RenderTxt(string text, Vector3 startPos = default)
    {
        base.RenderTxt(text, startPos);

        if (_isStatic)
        {
            TweenPunchScale(.35f); 
            return;
        }

        _image.enabled = true;
    }
    private void TweenPunchScale(float scale, int vibrato = 1)
    {
        transform.DORewind();
        transform.DOKill();
        transform.DOPunchScale(Vector3.one * scale, .5f, vibrato);
    }
    protected override void DeactiveAdded()
    {
        base.DeactiveAdded();

        if (_isStatic) return;

        _image.enabled = false;
    }
}

