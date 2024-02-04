using DG.Tweening;
using TMPro;
using UnityEngine;

public class UICoinsRender : MonoBehaviour
{
    [SerializeField] private bool _isStatic;
    [SerializeField] private RectTransform _endPos; 

    private TextMeshProUGUI _text;
    private const float tween_duration = 1f;
    private const float randomPos_offsetY = 30;
    private const float randomPos_offsetX = 50;
    private const float text_lifeTime = .35f; 

    private void OnEnable()
    {
        if (!_isStatic)
            UICoinsRenderController.RegistAddedCoinsUIRender(this);
        else
            UICoinsRenderController.RegistStaticCoinsUIRender(this);
    }
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }
    public void RenderTxt(string text, Vector3 startPos = new Vector3())
    {
        _text.text = text;

        if (!_isStatic)
        {
            _text.rectTransform.DOKill();
            CancelInvoke(nameof(DeactiveText)); 

            float xRandom = Random.Range(_endPos.position.x - randomPos_offsetX, _endPos.position.x + randomPos_offsetX);
            float yRandom = Random.Range(_endPos.position.y - randomPos_offsetY, _endPos.position.y + randomPos_offsetY);
            Vector3 startScreenPosition = Camera.main.WorldToScreenPoint(startPos);
            Vector3 endRandomPos = new Vector3(xRandom, yRandom, _endPos.position.z);

            _text.rectTransform.position = startScreenPosition;
            _text.rectTransform.localScale = Vector3.zero;
            _text.rectTransform.DOMove(endRandomPos, tween_duration)
                .SetEase(Ease.Flash);
            _text.rectTransform.DOScale(Vector3.one, tween_duration)
                .OnComplete(() => Invoke(nameof(DeactiveText), text_lifeTime));
        }
    }

    private void DeactiveText()
    {
        _text.rectTransform.DOScale(Vector3.zero, tween_duration * tween_duration)
            .OnComplete(() => _text.text = "");
    }
}
