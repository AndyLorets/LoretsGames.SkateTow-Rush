using System.Collections;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class UIGameTimeRenderer : MonoBehaviour
{
    [SerializeField] private UIMoveTween _uiMoveTween;

    private TextMeshProUGUI _text;
    private static float _gameTime; 

    protected void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();    
        GameManager.onGameStart += Show;
        GameManager.onFinish += Hide;
    }
    private void OnDestroy()
    {
        GameManager.onGameStart -= Show;
        GameManager.onFinish -= Hide;
    }
    private IEnumerator Timer()
    {
        float incriment = .1f; 
        WaitForSeconds waitForSeconds = new WaitForSeconds(incriment);
        while (GameManager.isGameStart)
        {
            _gameTime += incriment; 
            _text.text = ScoreText();
            yield return waitForSeconds;
        }
    }
    private void Hide() => _uiMoveTween.Hide();
    private void Show()
    {
        _uiMoveTween.Show();
        StartCoroutine(Timer());
    }
    static string ScoreString => TextTranslator.CurrentTextLanguage("Score: ", "Очки: ");
    public static string ScoreText()
    {
        float t = _gameTime;
        string minutes = Mathf.Floor(t / 60).ToString("00");
        string seconds = Mathf.Floor(t % 60).ToString("00");
        string milliseconds = Mathf.Floor((t * 100) % 100).ToString("00");
        string text = ScoreString + minutes + ":" + seconds + ":" + milliseconds;
        return text;
    }
    public static void Reset()
    {
        _gameTime = 0;
    }
}
