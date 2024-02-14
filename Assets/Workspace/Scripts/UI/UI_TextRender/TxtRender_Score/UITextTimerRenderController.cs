using System.Collections.Generic;
using UnityEngine;

public static class UITextTimerRenderController 
{
    private static UITextTimerRenderer _staticScoreUIRenderer;    
    private static List<UITextTimerRenderer> _addedScoreUIRenderList = new List<UITextTimerRenderer>();

    private static int _currentTextRendererIndex;

    public static void Init()
    {
        TimerManager.onTimeChanged += Render; 
    }
    public static void RegistStaticUIRender(UITextTimerRenderer uIText)
    {
        _staticScoreUIRenderer = uIText;
    }
    public static void RegistAddedUIRender(UITextTimerRenderer uIText) => _addedScoreUIRenderList.Add(uIText);
    private static void Render(bool isAdded)
    {
        string textAdded = $"+{1}";
        string textStatic = $"{TimerManager.gameTimer}";
        RenderStatic(textStatic);
        if (isAdded)
            RenderAdded(textAdded, Vector3.zero);
    }
    private static void RenderAdded(string text, Vector3 pos)
    {
        _addedScoreUIRenderList[_currentTextRendererIndex].RenderTxt(text, pos);

        if (_currentTextRendererIndex < _addedScoreUIRenderList.Count - 1)
            _currentTextRendererIndex++;
        else _currentTextRendererIndex = 0; 
    }
    private static void RenderStatic(string text)
    {
        _staticScoreUIRenderer.RenderTxt(text); 
    }
    public static void Reset()
    {
        _staticScoreUIRenderer = null;
        _addedScoreUIRenderList.Clear();
        TimerManager.onTimeChanged -= Render;
    }
}
