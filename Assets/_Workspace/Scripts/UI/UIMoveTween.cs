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
    [SerializeField] private bool _bouncedScale;
    [SerializeField] private bool _onlyScale;

    private RectTransform _rectTransform;
    private Vector3 _hiddenPosition;

    private RectTransform _hiddenPoint;
    private RectTransform _shownPoint;

    private Sequence _showAniamationTween;
    private Sequence _hideAniamationTween;

    private bool _isInitialized = false;

    private bool _isShown = false;

    private const float bounce_scale_cofficient = 1.2f;
    private const float bounce_duration_cofficient = .5f;
    public void HideShow()
    {
        if (_isShown) Hide(); 
        else Show();
    }
    public void Show()
    {
        if (_isShown) return;

        Start();

        _isShown = true;

        gameObject.SetActive(true);

        transform.DOKill();

        if (_unscaled)
        {
            if (!_onlyScale)
                transform.DOMove(_shownPoint.position, duration).SetEase(_moveEase).SetUpdate(true).OnComplete(() => { transform.position = _shownPoint.transform.position; });

            Vector3 bounceScale = _bouncedScale ? _showScale * bounce_scale_cofficient : _showScale;
            float bounceDuration = _bouncedScale ? duration * bounce_duration_cofficient : duration;
            transform.DOScale(bounceScale, duration)
                .SetEase(_moveEase)
                .SetUpdate(true)
                .OnComplete(delegate ()
                {
                    if (!_bouncedScale) return;
                    transform.DOScale(_showScale, bounceDuration).SetEase(_moveEase).SetUpdate(true);
                });
        }
        else
        {
            if (!_onlyScale)
                transform.DOMove(_shownPoint.position, duration).SetEase(_moveEase).OnComplete(() => { transform.position = _shownPoint.transform.position; });
            transform.DOScale(_showScale, duration).SetEase(_moveEase);
        }
    }

    public void Hide()
    {
        if (!_isShown) return;

        Start();

        _isShown = false;

        transform.DOKill();

        if (_deactivateOnHide)
        {
            if (_unscaled)
            {
                if (!_onlyScale)
                    transform.DOMove(_hiddenPoint.position, duration).SetEase(_moveEase).SetUpdate(true).OnComplete(() => { transform.position = _hiddenPoint.transform.position; });

                Vector3 bounceScale = _bouncedScale ? _showScale * bounce_scale_cofficient : _hideScale;
                float bounceDuration = _bouncedScale ? duration * bounce_duration_cofficient : duration;
                transform.DOScale(bounceScale, bounceDuration).SetEase(_moveEase)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        if (!_bouncedScale)
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                        transform.DOScale(_hideScale, duration).SetEase(_moveEase).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
                    });
            }
            else
            {
                if (!_onlyScale)
                    transform.DOMove(_hiddenPoint.position, duration).SetEase(_moveEase).OnComplete(() => { transform.position = _hiddenPoint.transform.position; });
                transform.DOScale(_hideScale, duration).SetEase(_moveEase).OnComplete(() => { gameObject.SetActive(false); });
            }
        }
        else
        {
            if (_unscaled)
            {
                if (!_onlyScale)
                    transform.DOMove(_hiddenPoint.position, duration).SetEase(_moveEase).SetUpdate(true);
                transform.DOScale(_hideScale, duration).SetEase(_moveEase).SetUpdate(true).OnComplete(() => { transform.position = _hiddenPoint.transform.position; });
            }
            else
            {
                if (!_onlyScale)
                    transform.DOMove(_hiddenPoint.position, duration).SetEase(_moveEase);
                transform.DOScale(_hideScale, duration).SetEase(_moveEase).OnComplete(() => { transform.position = _hiddenPoint.transform.position; });
            }
        }
    }

    public void Initialize()
    {
        if (_isInitialized) return;

        _rectTransform = transform as RectTransform;
        _hiddenPosition = new Vector3(transform.position.x + _startBias.x, transform.position.y + _startBias.y, transform.position.z + _startBias.z);

        if (_unscaled)
        {
            _showAniamationTween.SetUpdate(true);
            _hideAniamationTween.SetUpdate(true);
        }

        _shownPoint = CreatePoint("tween_Shown Point");

        if (_atStartPosition)
        {
            if (!_onlyScale)
                transform.position = _hiddenPosition;
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

        if (_deactivateOnHide && _atStartPosition)
        {
            gameObject.SetActive(false);
        }

        _isInitialized = true;
    }

    private void Start()
    {
        Initialize();
    }

    private RectTransform CreatePoint(string name)
    {
        Transform target = new GameObject(name, typeof(RectTransform)).transform;
        target.SetParent(transform.parent, false);

        RectTransform copy = target as RectTransform;

        copy.SetParent(transform.parent, false);
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
