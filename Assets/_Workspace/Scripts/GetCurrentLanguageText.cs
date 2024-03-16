using GamePush;
using UnityEngine;

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
