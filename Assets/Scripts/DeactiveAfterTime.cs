using UnityEngine;

public class DeactiveAfterTime : MonoBehaviour
{
    [SerializeField] private float _duration = 3f;
    private void Start() => Invoke(nameof(Dective), _duration);
    private void Dective() => gameObject.SetActive(false);

}
