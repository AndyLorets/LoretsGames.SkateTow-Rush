using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmojiBehaviour : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GetCurrentLanguageText _currentLanguageText;
    [Space(10)]
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _emoji;

    public static System.Action<string, Sprite> onEmoji;

    private bool _isChanged; 

    private Player _player;

    public static System.Action OnSpeedChange;

    private const float min_scoreSpeed = 25f;
    private const float tween_duration = .7f;

    private bool _first = true; 

    private void Start()
    {
        _player = ServiceLocator.GetService<Player>();
    }
    private void OnEnable()
    {
        DisableCanvas();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ObjTags.player_tag)) return;

        if (_player.SpeedScore > min_scoreSpeed && !_isChanged)
        {
            _isChanged = true;
            OnSpeedChange?.Invoke();

            EnableCanvas(); 
        }
    }
    private void EnableCanvas()
    {
        _rect.gameObject.SetActive(true);

        _rect.DOScale(Vector3.one * 1.1f, tween_duration)
            .OnComplete(() => _rect.DOScale(Vector3.one, tween_duration * .5f));

        _text.text = _currentLanguageText.GetTextRandom();
        _image.sprite = _emoji[Random.Range(0, _emoji.Length)];

        onEmoji?.Invoke(_text.text, _image.sprite); 

        Invoke(nameof(DisableCanvas), 3f); 
    }
    private void DisableCanvas()
    {
        if (_first)
        {
            _rect.gameObject.SetActive(false);
            _first = false; 
        }

        _rect.DOScale(Vector3.one * 1.1f, tween_duration * .5f)
            .OnComplete(delegate ()
            {
                _rect.DOScale(Vector3.zero, tween_duration);
                _rect.gameObject.SetActive(false);  
            });
    }
    public void Reset()
    {
        _isChanged = false; 
    }
}
