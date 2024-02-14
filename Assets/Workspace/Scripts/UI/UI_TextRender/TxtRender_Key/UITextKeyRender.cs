public class UITextKeyRender : UITextRendererBase
{
    protected override void Regist()
    {
        if (!_isStatic)
            UITextKeyRenderController.RegistAddedKeyUIRender(this);
        else
            UITextKeyRenderController.RegistStaticKeyUIRender(this);
    }
}

