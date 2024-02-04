using DG.Tweening; 
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ForegroundShadow : MonoBehaviour
{
    [SerializeField] private Color _color = Color.black;

    private Image _image; 
    
    private void Awake()
    {
        GameManager.onNextLevel += SetColor;
        GameManager.onRestart += SetColor;
        Construct(); 
    }
    private void Construct()
    {
        _image = GetComponent<Image>();
        SetFirstColor(); 

    }
    private void SetFirstColor()
    {
        _image.enabled = true; 
        _image.color = _color;
        _image.DOColor(Color.clear, GameManager.nextLevel_fade_duration)
            .OnComplete(() => _image.enabled = false);
    }
    private void SetColor()
    {
        _image.enabled = true;
        _image.DOColor(_color, GameManager.nextLevel_fade_duration);
    }
    private void OnDestroy()
    {
        GameManager.onNextLevel -= SetColor;
        GameManager.onRestart -= SetColor;
    }
}
