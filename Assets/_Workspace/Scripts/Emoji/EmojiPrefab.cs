using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmojiPrefab : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private UIMoveTween _moveTween;

    private const float active_duration = 3f; 

    public bool isActive { get; private set; }

    public void Render(string text, Sprite sprite)
    {
        _text.text = text;
        _image.sprite = sprite;

        Show(); 
    }
    private void Show()
    {
        _moveTween.Show();
        isActive = true; 
        Invoke(nameof(Hide), active_duration); 
    }
    public void Hide()
    {
        _moveTween.Hide();
        isActive = false;
    }
}
