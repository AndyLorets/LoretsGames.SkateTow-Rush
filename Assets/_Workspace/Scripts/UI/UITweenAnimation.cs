using DG.Tweening;
using UnityEngine;

public class UITweenAnimation : MonoBehaviour
{
    [SerializeField] private bool _playOnStart;
    [Space(10)]
    [SerializeField] private bool _playScaleAnimation;
    [SerializeField] private ScaleTweenAnimation _scaleTweenAnimation;
    [Space(7)]
    [SerializeField] private bool _playPosTweenAnimation;
    [SerializeField] private PosTweenAnimation _posTweenAnimation;
    [Space(7)]
    [SerializeField] private bool _playLocPosTweenAnimation;
    [SerializeField] private LocPosTweenAnimation _locPosTweenAnimation;

    private void Start()
    {
        if (!_playOnStart) return;

        PlayScaleTween();
        PlayePosTween(); 
        PlayeLocalPosTween(); 
    }
    public void PlayScaleTween()
    {
        if (!_playScaleAnimation) return;

        _scaleTweenAnimation.SetAnimation(transform, transform.localScale);
    }
    public void PlayePosTween()
    {
        if (!_playPosTweenAnimation) return;

        _posTweenAnimation.SetAnimation(transform, transform.localPosition);
    }
    public void PlayeLocalPosTween()
    {
        if (!_playLocPosTweenAnimation) return;

        _locPosTweenAnimation.SetAnimation(transform, transform.localPosition);
    }
    private void OnDisable()
    {
        if (_playScaleAnimation)
            _scaleTweenAnimation.OnDisable(transform);

        if (_playLocPosTweenAnimation)
            _locPosTweenAnimation.OnDisable(transform);
    }
}

[System.Serializable]
public class ScaleTweenAnimation
{
    [SerializeField] private bool _loop;
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _endScale;

    public void SetAnimation(Transform transform, Vector3 startScale)
    {
        transform.DOScale(_endScale, _duration / 2).SetUpdate(true)
            .OnComplete(() => transform.DOScale(startScale, _duration / 2).SetUpdate(true));

        DOTween.To(() => _duration, x => _duration = x, _duration, _duration)
        .OnComplete(delegate ()
        {
            if (_loop)
                SetAnimation(transform, startScale);
        });
    }
    public void OnDisable(Transform transform)
    {
        transform.DORewind();
        transform.DOKill();
    }
}

[System.Serializable]
public class PosTweenAnimation
{
    [SerializeField] private bool _loop;
    [SerializeField] private bool _backToPos;
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _offset;

    public void SetAnimation(Transform transform, Vector3 startPos)
    {
        _offset += transform.position; 
        transform.DOMove(_offset, _duration / 2).SetUpdate(true)
            .OnComplete(delegate ()
            {
                if (_backToPos)
                    transform.DOLocalMove(startPos, _duration / 2).SetUpdate(true); 
            });


        DOTween.To(() => _duration, x => _duration = x, _duration, _duration)
            .OnComplete(delegate ()
            {
                if (_loop)
                    SetAnimation(transform, startPos);
            });
    }
    public void OnDisable(Transform transform)
    {
        transform.DOKill();
        transform.DORewind();
    }
}

[System.Serializable]
public class LocPosTweenAnimation
{
    [SerializeField] private bool _loop;
    [SerializeField] private bool _backToPos;
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _offset;

    public void SetAnimation(Transform transform, Vector3 startPos)
    {
        _offset += transform.position; 
        transform.DOLocalMove(_offset, _duration / 2).SetUpdate(true)
            .OnComplete(delegate ()
            {
                if (_backToPos)
                    transform.DOLocalMove(startPos, _duration / 2).SetUpdate(true);
            });


        DOTween.To(() => _duration, x => _duration = x, _duration, _duration)
            .OnComplete(delegate ()
            {
                if (_loop)
                    SetAnimation(transform, startPos);
            });
    }
    public void OnDisable(Transform transform)
    {
        transform.DOKill();
        transform.DORewind();
    }
}
