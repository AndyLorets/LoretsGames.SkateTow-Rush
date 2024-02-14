using System.Collections.Generic;
using UnityEngine;

public static class UITextCoinsRenderController 
{
    private static UITextCoinsRender _staticCoinsUIRenderer;
    private static List<UITextCoinsRender> _addedCoinsUIRenderList = new List<UITextCoinsRender>();

    private static int _currentTextRendererIndex;

    public static void Init()
    {
        CoinsManager.onChangeCoins += Render; 
    }

    public static void RegistDynamicUICoinsRender(UITextCoinsRender uIText)
    {
        _addedCoinsUIRenderList.Add(uIText);
    }
    public static void RegistStaticUICoinsRender(UITextCoinsRender uIText)
    {
        _staticCoinsUIRenderer = uIText;
        RenderStaticCoins();
    }
    public static void Render(int value, Vector3 startPos)
    {
        RenderAddedCoins(value, startPos);
        RenderStaticCoins(); 
    }
    private static void RenderAddedCoins(int value, Vector3 startPos)
    {
        string textAddedCoins = $"+{value}$";
        _addedCoinsUIRenderList[_currentTextRendererIndex].RenderTxt(textAddedCoins, startPos);

        if (_currentTextRendererIndex < _addedCoinsUIRenderList.Count - 1)
            _currentTextRendererIndex++;
        else _currentTextRendererIndex = 0;
    }
    private static void RenderStaticCoins()
    {
        string text = $"{GameDataManager.CoinsCount + CoinsManager.currentCoinsCount}";
        _staticCoinsUIRenderer.RenderTxt(text);
    }
    public static void Reset()
    {
        _addedCoinsUIRenderList.Clear();
        _staticCoinsUIRenderer = null;

        CoinsManager.onChangeCoins -= Render;
    }
}
