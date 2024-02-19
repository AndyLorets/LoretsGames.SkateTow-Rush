using System.Collections.Generic;
using UnityEngine;

public static class UIMoneyRenderController 
{
    private static UIMoneyRender _staticUIRenderer;
    private static List<UIMoneyRender> _addedUIRenderList = new List<UIMoneyRender>();

    private static int _currentTextRendererIndex;

    public static void Init()
    {
        MoneyManager.onChange += Render; 
    }

    public static void RegistDynamicUIRender(UIMoneyRender uIText)
    {
        _addedUIRenderList.Add(uIText);
    }
    public static void RegistStaticUIRender(UIMoneyRender uIText)
    {
        _staticUIRenderer = uIText;
        RenderStatic();
    }
    public static void Render(int value, Vector3 startPos)
    {
        RenderAdded(value, startPos);
        RenderStatic(); 
    }
    private static void RenderAdded(int value, Vector3 startPos)
    {
        string textAddedCoins = $"+{value}$";
        _addedUIRenderList[_currentTextRendererIndex].RenderTxt(textAddedCoins, startPos);

        if (_currentTextRendererIndex < _addedUIRenderList.Count - 1)
            _currentTextRendererIndex++;
        else _currentTextRendererIndex = 0;
    }
    private static void RenderStatic()
    {
        string text = $"{GameDataManager.MoneyCount + MoneyManager.currentCount}";
        _staticUIRenderer.RenderTxt(text);
    }
    public static void Reset()
    {
        _addedUIRenderList.Clear();
        _staticUIRenderer = null;

        MoneyManager.onChange -= Render;
    }
}
