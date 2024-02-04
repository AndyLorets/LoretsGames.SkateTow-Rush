using System.Collections.Generic;
using UnityEngine;

public static class UIScoreRenderController 
{
    private static UIScoreRender _staticScoreUIRenderer;    
    private static List<UIScoreRender> _addedScoreUIRenderList = new List<UIScoreRender>();

    private static int _currentTextRendererIndex;

    private static string[] _OnLoseComments =
{
        "L02e1*#",
        "@MG!*.",
        "F06k!",
        "D#m6@",
    };
    public static void Init()
    {
        ScoreManager.onAddScore += Render; 
    }

    public static void RegistAddedScoreUIRender(UIScoreRender uIText) => _addedScoreUIRenderList.Add(uIText);
    public static void RegistStaticScoreUIRender(UIScoreRender uIText) => _staticScoreUIRenderer = uIText;
    private static void Render(int addScore, Vector3 pos)
    {
        bool lose = addScore <= 0;
        string textAddScore = lose ? _OnLoseComments[Random.Range(0, _OnLoseComments.Length)] : $"+{addScore}";
        string textStaticScore = $"Score: {ScoreManager.Score}";
        RenderAddedScore(textAddScore, pos, lose);
        RenderStaticScore(textStaticScore);
    }
    private static void RenderAddedScore(string text, Vector3 pos, bool lose)
    {
        _addedScoreUIRenderList[_currentTextRendererIndex].RenderTxt(text, lose, pos);

        if (_currentTextRendererIndex < _addedScoreUIRenderList.Count - 1)
            _currentTextRendererIndex++;
        else _currentTextRendererIndex = 0; 
    }
    private static void RenderStaticScore(string text)
    {
        _staticScoreUIRenderer.RenderTxt(text); 
    }
    public static void Reset()
    {
        _staticScoreUIRenderer = null;
        _addedScoreUIRenderList.Clear();
        ScoreManager.onAddScore -= Render;
    }

}
