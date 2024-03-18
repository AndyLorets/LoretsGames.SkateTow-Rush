using DG.Tweening;
using TMPro;
using UnityEngine;

public abstract class UIRendererBase : MonoBehaviour
{
    [SerializeField] protected bool _isStatic;
    [Space(5)]
    [SerializeField] private Ease _tweenEasy; 
    [SerializeField] private RectTransform _endPos;

    private RectTransform _rectTransform;

    protected TextMeshProUGUI _text;
    private const float tween_duration = 1f;
    private const float randomPos_offsetY = 30;
    private const float randomPos_offsetX = 50;
    private const float text_lifeTime = .1f;

    protected virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>(); 
        _text = GetComponent<TextMeshProUGUI>();

        if(_isStatic)
            _text.enabled = true;
    }
    private void Start()
    {
        Invoke(nameof(Regist), .1f); 
    }
    protected abstract void Regist(); 
    public virtual void RenderTxt(string text, Vector3 startPos = new Vector3())
    {
        if (_isStatic) 
        { 
            _text.text = text;

            return; 
        }

        _rectTransform.DOKill();
        CancelInvoke(nameof(DeactiveAdded));

        //float xRandom = Random.Range(_offset.position.x - randomPos_offsetX, _offset.position.x + randomPos_offsetX);
        //float yRandom = Random.Range(_offset.position.y - randomPos_offsetY, _offset.position.y + randomPos_offsetY);
        Vector3 startScreenPosition = Camera.main.WorldToScreenPoint(startPos);
        //Vector3 endRandomPos = new Vector3(xRandom, yRandom, _offset.position.z);

        _rectTransform.position = startScreenPosition;
        _rectTransform.localScale = Vector3.zero;
        _rectTransform.DOMove(_endPos.position, tween_duration)
            .SetEase(_tweenEasy);
        _rectTransform.DOScale(Vector3.one, tween_duration)
            .OnComplete(() => Invoke(nameof(DeactiveAdded), text_lifeTime));

    }
    protected virtual void DeactiveAdded()
    {
        _rectTransform.DOScale(Vector3.zero, tween_duration * .5f);
    }
}
