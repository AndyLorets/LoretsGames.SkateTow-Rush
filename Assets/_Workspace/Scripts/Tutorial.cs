using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private UIMoveTween _uiMoveTween;
    [SerializeField] private Image _bg;
    [SerializeField] private GameObject[] _pages;

    private Color _bgColor;
    private int _currentPage; 

    private const float tween_duration = 1f;
    private const string prefs_name = "Tutorial"; 

    void Start()
    {
        Construct();

        bool hasSave = PlayerPrefs.HasKey(prefs_name); 
        if (!hasSave)
        {
            Invoke(nameof(Show), 2f);
            PlayerPrefs.SetInt(prefs_name, 1); 
        }
    }
    private void Construct()
    {
        _bgColor = _bg.color;
        _bg.color = Color.clear;
        _bg.enabled = false; 

        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i].SetActive(false); 
        }
        _pages[0].SetActive(true);
    }
    public void Show()
    {
        _bg.enabled = true;
        _bg.DOColor(_bgColor, tween_duration / 2f).SetUpdate(true); 
        _uiMoveTween.Show();
    }
    public void Hide()
    {
        _bg.DOColor(Color.clear, tween_duration).SetUpdate(true).OnComplete(() => _bg.enabled = false);
        _uiMoveTween.Hide();
    }
    public void NextPage()
    {
        if (_currentPage < _pages.Length - 1)
        {
            _currentPage++;
            _pages[_currentPage - 1].SetActive(false);
            _pages[_currentPage].SetActive(true);
        }
        else
        {
            Hide();

            for (int i = 0; i < _pages.Length; i++)
            {
                _pages[i].SetActive(false);
            }
            _currentPage = 0;
            _pages[0].SetActive(true);
        }
    }
}
