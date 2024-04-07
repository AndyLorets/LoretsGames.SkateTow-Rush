using System.Collections;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class UIGameTimeRenderer : MonoBehaviour
{
    [SerializeField] private UIMoveTween _uiMoveTween;

    private TextMeshProUGUI _text;

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
        WaitForSeconds waitForSeconds = new WaitForSeconds(.1f);
        while (GameManager.isGameStart)
        {
            _text.text = GameTimeText();
            yield return waitForSeconds;
        }
    }
    private void Hide() => _uiMoveTween.Hide();
    private void Show()
    {
        _uiMoveTween.Show();
        StartCoroutine(Timer());
    }
    public static string GameTimeText()
    {
        float t = Time.time;
        string minutes = Mathf.Floor(t / 60).ToString("00");
        string seconds = Mathf.Floor(t % 60).ToString("00");
        string milliseconds = Mathf.Floor((t * 100) % 100).ToString("00");
        string text = minutes + ":" + seconds + ":" + milliseconds;
        return text;
    }
}
