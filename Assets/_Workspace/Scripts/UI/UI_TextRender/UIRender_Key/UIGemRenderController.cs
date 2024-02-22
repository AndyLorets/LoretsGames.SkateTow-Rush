using System.Collections.Generic;
using UnityEngine; 
public static class UIGemRenderController 
{
    private static UIGemRender _staticGemUIRenderer;
    private static List<UIGemRender> _addedGemUIRenderList = new List<UIGemRender>();
    private static int _currentTextRendererIndex;

    public static void Init()
    {
        GemManager.onChangeValue += Render;
    }
    public static void RegistAddedUIRender(UIGemRender uIText)
    {
        _addedGemUIRenderList.Add(uIText);
    }
    public static void RegistStaticUIRender(UIGemRender uIText)
    {
        _staticGemUIRenderer = uIText;
        RenderStatic();
    }
    private static void Render(int addKey, Vector3 pos)
    {
        RenderAdded($"+{addKey}", pos);
        RenderStatic(); 
    }
    private static void RenderAdded(string text, Vector3 pos)
    {
        _addedGemUIRenderList[_currentTextRendererIndex].RenderTxt(text, pos);

        if (_currentTextRendererIndex < _addedGemUIRenderList.Count - 1)
            _currentTextRendererIndex++;
        else _currentTextRendererIndex = 0;
    }
    private static void RenderStatic()
    {
        string text = $"{GameDataManager.GemCount + GemManager.currentGemCount}";
        _staticGemUIRenderer.RenderTxt(text);
    }
    public static void Reset()
    {
        _addedGemUIRenderList.Clear(); 
        _staticGemUIRenderer = null;
        GemManager.onChangeValue -= Render;
    }

}
