using DG.Tweening;
using UnityEngine;

public class ColorTween : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _targetColor = new Color(1, 1, 1, 1);

    private Color _startColor; 

    private const float tween_duration = .65f; 

    void Start()
    {
        _meshRenderer.material = Instantiate(_meshRenderer.material);
        _startColor = _meshRenderer.material.color;

        ColorTweening(); 
    }

   private void ColorTweening()
    {
        _meshRenderer.material.DOColor(_targetColor, tween_duration).OnComplete(() => _meshRenderer.material.DOColor(_startColor, tween_duration).OnComplete(() => ColorTweening())); 
    }
}
