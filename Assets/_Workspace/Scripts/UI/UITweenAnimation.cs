using DG.Tweening;
using UnityEngine;

public class UITweenAnimation : MonoBehaviour
{
    [SerializeField] private bool _playOnStart;
    [Space(10)]
    [SerializeField] private bool _playScaleAnimation;
    [SerializeField] private ScaleTweenAnimation _scaleTweenAnimation;
    [Space(7)]
    [SerializeField] private bool _playLocPosTweenAnimation;
    [SerializeField] private LocPosTweenAnimation _locPosTweenAnimation;

    private void Start()
    {
        if (!_playOnStart) return;

        if (_playScaleAnimation)
            _scaleTweenAnimation.SetAnimation(transform, transform.localScale);

        if (_playLocPosTweenAnimation)
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
public class LocPosTweenAnimation
{
    [SerializeField] private bool _loop;
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _endPos;

    public void SetAnimation(Transform transform, Vector3 startPos)
    {
        transform.DOLocalMove(_endPos, _duration / 2).SetUpdate(true)
            .OnComplete(() => transform.DOLocalMove(startPos, _duration / 2).SetUpdate(true));


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
