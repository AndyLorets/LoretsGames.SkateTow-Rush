using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Animator _animator; 
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private Collider[] _colliders;

    private const float impulse_power = 3.5f;
    private const float disable_duration = 8.0f;

    private Collider _collider; 

    private bool _isRagdoll; 

    private void Start()
    {
        _collider = GetComponent<Collider>();
        SetState(false); 
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag(ObjTags.player_tag)) return;

        if (!_isRagdoll)
            SetState(true);
    }
    public void SetState(bool state)
    {
        _isRagdoll = state; 

        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _animator.enabled = !state;
            _rigidbodies[i].isKinematic = !state;
            _colliders[i].enabled = state;
            _collider.enabled = !state;

            if (state)
            {
                _rigidbodies[i].AddForce(Vector3.up * impulse_power, ForceMode.Impulse);
                _rigidbodies[i].AddTorque(Vector3.up * impulse_power, ForceMode.Impulse);
            }
        }

        if(state)
            Invoke(nameof(Disable), disable_duration);
    }
    private void Disable() => gameObject.SetActive(false);
}
