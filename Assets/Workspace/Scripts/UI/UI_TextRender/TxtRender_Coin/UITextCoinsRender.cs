public class UITextCoinsRender : UITextRendererBase
{
    protected override void Regist()
    {
        if (_isStatic)
            UITextCoinsRenderController.RegistStaticUICoinsRender(this);
        else
            UITextCoinsRenderController.RegistDynamicUICoinsRender(this);
    }
}
