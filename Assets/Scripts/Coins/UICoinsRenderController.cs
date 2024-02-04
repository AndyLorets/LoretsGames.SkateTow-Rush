using System.Collections.Generic;
using UnityEngine;

public static class UICoinsRenderController
{
    private static UICoinsRender _staticCoinsUIRenderer;
    private static List<UICoinsRender> _addedCoinsUIRenderList = new List<UICoinsRender>();

    private static int _currentTextRendererIndex;

    public static void RegistAddedCoinsUIRender(UICoinsRender uIText) => _addedCoinsUIRenderList.Add(uIText);
    public static void RegistStaticCoinsUIRender(UICoinsRender uIText) => _staticCoinsUIRenderer = uIText;

    public static void RenderAddedCoins(string text, Vector3 startPos)
    {
        _addedCoinsUIRenderList[_currentTextRendererIndex].RenderTxt(text, startPos);

        if (_currentTextRendererIndex < _addedCoinsUIRenderList.Count - 1)
            _currentTextRendererIndex++;
        else _currentTextRendererIndex = 0;
    }
    public static void RenderStaticCoins(string text)
    {
        _staticCoinsUIRenderer.RenderTxt(text);
    }
    public static void Reset()
    {
        _staticCoinsUIRenderer = null;
        _addedCoinsUIRenderList.Clear();
    }
}
