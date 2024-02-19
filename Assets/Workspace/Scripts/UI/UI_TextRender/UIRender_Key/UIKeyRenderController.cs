using System.Collections.Generic;
using UnityEngine; 
public static class UIKeyRenderController 
{
    private static UIKeyRender _staticKeyUIRenderer;
    private static List<UIKeyRender> _addedKeyUIRenderList = new List<UIKeyRender>();
    private static int _currentTextRendererIndex;

    public static void Init()
    {
        KeyManager.onChangeKey += Render;
    }
    public static void RegistAddedUIRender(UIKeyRender uIText)
    {
        _addedKeyUIRenderList.Add(uIText);
    }
    public static void RegistStaticUIRender(UIKeyRender uIText)
    {
        _staticKeyUIRenderer = uIText;
        RenderStaticKey();
    }
    private static void Render(int addKey, Vector3 pos)
    {
        RenderAddedKey($"+{addKey}", pos);
        RenderStaticKey(); 
    }
    private static void RenderAddedKey(string text, Vector3 pos)
    {
        _addedKeyUIRenderList[_currentTextRendererIndex].RenderTxt(text, pos);

        if (_currentTextRendererIndex < _addedKeyUIRenderList.Count - 1)
            _currentTextRendererIndex++;
        else _currentTextRendererIndex = 0;
    }
    private static void RenderStaticKey()
    {
        string text = $"{GameDataManager.KeysCount + KeyManager.currentKeysCount}";
        _staticKeyUIRenderer.RenderTxt(text);
    }
    public static void Reset()
    {
        _addedKeyUIRenderList.Clear(); 
        _staticKeyUIRenderer = null;
        KeyManager.onChangeKey -= Render;
    }

}
