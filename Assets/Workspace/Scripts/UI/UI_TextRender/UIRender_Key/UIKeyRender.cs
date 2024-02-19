using UnityEngine;
using UnityEngine.UI;
public class UIKeyRender : UIRendererBase
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
            UIKeyRenderController.RegistAddedUIRender(this);
        else
            UIKeyRenderController.RegistStaticUIRender(this);
    }
    public override void RenderTxt(string text, Vector3 startPos = default)
    {
        base.RenderTxt(text, startPos);

        if (_isStatic) return;

        _image.enabled = true;
    }
    protected override void DeactiveAdded()
    {
        base.DeactiveAdded();

        if (_isStatic) return;

        _image.enabled = false;
    }
}

