using DG.Tweening;
using TMPro;
using UnityEngine;

public class ColorTween : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _targetColor = new Color(1, 1, 1, 1);

    private Color _startColor; 

    private const float tween_duration = .65f; 

    void Awake()
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.material = Instantiate(_meshRenderer.material);
            _startColor = _meshRenderer.material.color;
        }
        if (_text != null)
            _startColor = _text.color;
    }

    private void OnEnable()
    {


        ColorTweening();
    }
    private void OnDisable()
    {
        TweenKill(); 
    }
    private void ColorTweening()
    {
        if(_meshRenderer != null)
            _meshRenderer.material.DOColor(_targetColor, tween_duration).OnComplete(() => _meshRenderer.material.DOColor(_startColor, tween_duration).OnComplete(() => ColorTweening()));

        if(_text != null )
            _text.DOColor(_targetColor, tween_duration).OnComplete(() => _text.DOColor(_startColor, tween_duration).OnComplete(() => ColorTweening())); 
    }
    private void TweenKill()
    {
        if (_text != null)
            _text.DOKill();
        if (_meshRenderer != null)
            _meshRenderer.DOKill(); 
    }

}
