using UnityEngine;
using GamePush;
using UnityEngine.UI;
using TMPro; 

public class LanguagesText : MonoBehaviour
{
    [SerializeField] private string _ru;
    [SerializeField] private string _en;
    [Space(5)]
    [SerializeField] private Text _text;
    [SerializeField] private TextMeshProUGUI _textPro;
    private void Start()
    {
        if (_text == null)
            _text = GetComponent<Text>();
        if(_textPro == null )
            _textPro = GetComponent<TextMeshProUGUI>();

        RenderText();

        if (GameManager.DEBBUG_LOG)
            Debug.Log("Language: " + GP_Language.Current().ToString());
    }

    public void Change(Language language) => GP_Language.Change(language, OnChange);
    private void OnChange(Language language) 
    { 
        if (GameManager.DEBBUG_LOG) 
            Debug.Log("LANGUAGE : ON CHANGE: " + language); 
    }

    private void RenderText()
    {
        switch (GP_Language.Current())
        {
            case Language.English:
                if (_text != null)
                    _text.text = _en;
                if (_textPro != null)
                    _textPro.text = _en; 
                break;
            case Language.Russian:
                if (_text != null)
                    _text.text = _ru;
                if (_textPro != null)
                    _textPro.text = _ru;
                break;
        }
    }
}
public struct TextTranslator
{
    public static string CurrentTextLanguage(string en, string ru)
    {
        switch (GP_Language.Current())
        {
            case Language.English: return en;
            case Language.Russian: return ru;
        }
        return en; 
    }
}
[System.Serializable]
public struct GetCurrentLanguageText
{
    [SerializeField] private string[] _ruDescriptions;
    [SerializeField] private string[] _enDescriptions;

    public string GetTextFromIndex(int index)
    {
        string[] text = GP_Language.Current() == Language.English ? _enDescriptions : _ruDescriptions;
        return text[index];
    }
    public string GetTextRandom()
    {
        string[] text = GP_Language.Current() == Language.English ? _enDescriptions : _ruDescriptions;
        int r = Random.Range(0, text.Length);
        return text[r];
    }
}