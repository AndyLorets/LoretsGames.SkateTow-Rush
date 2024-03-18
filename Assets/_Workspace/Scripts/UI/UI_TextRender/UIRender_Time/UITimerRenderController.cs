using System.Collections.Generic;
using UnityEngine;

public static class UITimerRenderController
{
    private static UITimerRenderer _staticUIRenderer;
    private static UIGameTimeRenderer _staticGameTimeUIRenderer;
    private static List<UITimerRenderer> _addedUIRenderList = new List<UITimerRenderer>();

    private static int _currentTextRendererIndex;
    public static string GameTimeText()
    {
        float t = TimerManager.gameTime;
        string minutes = Mathf.Floor(t / 60).ToString("00");
        string seconds = Mathf.Floor(t % 60).ToString("00");
        string milliseconds = Mathf.Floor((t * 100) % 100).ToString("00");
        string text = minutes + ":" + seconds + ":" + milliseconds;
        return text;
    }
    public static void Init()
    {
        TimerManager.onTimeChanged += Render; 
    }
    public static void RegistAddedUIRender(UITimerRenderer uIText)
    {
        _addedUIRenderList.Add(uIText);
    }
    public static void RegistStaticUIRender(UITimerRenderer uIText)
    {
        _staticUIRenderer = uIText;
    }
    public static void RegistStaticUIRender(UIGameTimeRenderer uIText)
    {
        _staticGameTimeUIRenderer = uIText;
    }
    private static void Render(bool isAdded)
    {
        int itemValue = TimerManager.addTimeValue;// GameDataManager.UpgradeValue.GetValue(ItemConvertor.ConvertTitleFromType(ItemType.UpgradeTime)); 
        string textAdded = $"+{itemValue}";
        RenderStatic();
        if (isAdded)
            RenderAdded(textAdded, ServiceLocator.GetService<Player>().transform.position);
    }
    private static void RenderAdded(string text, Vector3 pos)
    {
        _addedUIRenderList[_currentTextRendererIndex].RenderTxt(text, pos);

        if (_currentTextRendererIndex < _addedUIRenderList.Count - 1)
            _currentTextRendererIndex++;
        else _currentTextRendererIndex = 0; 
    }
    private static void RenderStatic()
    {
        string timerText = $"{TimerManager.gameDeadLineTime}";
        string gameTimeText = GameTimeText();
        _staticUIRenderer.RenderTxt(timerText);
        _staticGameTimeUIRenderer.RenderTxt(gameTimeText); 
    }
    public static void Reset()
    {
        _staticUIRenderer = null;
        _addedUIRenderList.Clear();
        TimerManager.onTimeChanged -= Render;
    }
}
