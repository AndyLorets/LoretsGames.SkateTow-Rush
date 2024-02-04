using DG.Tweening;
using UnityEngine;

public class UIMoveTween : MonoBehaviour
{
    public float Duratuin => duration;

    [SerializeField] private Ease _moveEase = Ease.InOutSine;
    [SerializeField] private Vector3 _startBias;
    [SerializeField] private Vector3 _hideScale;
    [SerializeField] private Vector3 _showScale;
    [SerializeField] private float duration;
    [SerializeField] private bool _atStartPosition;
    [SerializeField] private bool _deactivateOnHide = false;
    [SerializeField] private bool _unscaled = false;
    [SerializeField] private bool _hideOnStart = true;
    [SerializeField] private bool _ignoreTimeScale = true; 

    private RectTransform _rectTransform;
    private RectTransform _hiddenPoint;
    private RectTransform _shownPoint;

    private Sequence _showAniamationTween;
    private Sequence _hideAniamationTween;

    private bool _isShown = false;

    public void Show()
    {
        if (_isShown) return;

        _isShown = true;

        gameObject.SetActive(true);

        transform.DOKill();

        transform.DOScale(_showScale, duration).SetEase(_moveEase)
            .SetUpdate(_ignoreTimeScale);
        transform.DOMove(_shownPoint.position, duration).SetEase(_moveEase)
            .SetUpdate(_ignoreTimeScale)
            .OnComplete(() => { transform.position = _shownPoint.transform.position; });
    }

    public void Hide()
    {
        if (!_isShown) return;

        DOTween.useSmoothDeltaTime = _ignoreTimeScale;

        _isShown = false;

        transform.DOKill();

        if (_deactivateOnHide)
        {
            transform.DOMove(_hiddenPoint.position, duration).SetEase(_moveEase)
                .SetUpdate(_ignoreTimeScale)
                .OnComplete(() => { transform.position = _hiddenPoint.transform.position; });
            transform.DOScale(_hideScale, duration).SetEase(_moveEase)
                .SetUpdate(_ignoreTimeScale)
                .OnComplete(() => { gameObject.SetActive(false); });
        }
        else
        {
            transform.DOMove(_hiddenPoint.position, duration)
                .SetUpdate(_ignoreTimeScale)
                .SetEase(_moveEase);
            transform.DOScale(_hideScale, duration)
                .SetUpdate(_ignoreTimeScale)
                .SetEase(_moveEase)
                .OnComplete(() => { transform.position = _hiddenPoint.transform.position; });
        }
    }

    private void Start()
    {
        _rectTransform = transform as RectTransform;

        if (_unscaled)
        {
            _showAniamationTween.SetUpdate(true);
            _hideAniamationTween.SetUpdate(true);
        }

        _shownPoint = CreatePoint("tween_Shown Point");

        if (_atStartPosition)
        {
            //transform.position = _hiddenPosition;
            transform.localScale = _hideScale;
            _isShown = false;

            _hiddenPoint = CreatePoint("tween_Hidden Point");
        }
        else
        {
            _isShown = true;

            _hiddenPoint = CreatePoint("tween_Hidden Point");
            _hiddenPoint.transform.position = _hiddenPoint.transform.position + _startBias;
        }

        if (_deactivateOnHide && _hideOnStart)
        {
            gameObject.SetActive(false);
        }
    }

    private RectTransform CreatePoint(string name)
    {
        Transform target = new GameObject(name, typeof(RectTransform)).transform;
        target.parent = transform.parent;

        RectTransform copy = target as RectTransform;

        copy.parent = transform.parent;
        copy.anchorMin = _rectTransform.anchorMin;
        copy.anchorMax = _rectTransform.anchorMax;
        copy.anchoredPosition = _rectTransform.anchoredPosition;

        return copy;
    }

    private void OnDestroy()
    {
        transform.DOKill();
        _showAniamationTween.Kill();
    }
}
