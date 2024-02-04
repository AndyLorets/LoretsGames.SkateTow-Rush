using UnityEngine;

public class RigidbodyConstraintsController : MonoBehaviour, ITakeDamage
{
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private RigidbodyConstraints _onStartConstraints = RigidbodyConstraints.FreezeAll;
    [SerializeField] private RigidbodyConstraints _onTriggerConstraints = RigidbodyConstraints.None;

    private const float rb_mass = 0.7f;
    private const float impulse_power = 4.0f;
    private const float destroy_duration = 3.0f;

    private void Start()
    {
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].constraints = _onStartConstraints;
            _rigidbodies[i].mass = rb_mass; 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ObjTags.player_tag)) return;

        TakeDamage(); 
    }

    public void TakeDamage(float damage = 1)
    {
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].constraints = _onTriggerConstraints;
            _rigidbodies[i].AddForce(Vector3.up * impulse_power, ForceMode.Impulse);
            _rigidbodies[i].AddTorque(Vector3.up * impulse_power, ForceMode.Impulse);

            Destroy(_rigidbodies[i], destroy_duration);
        }
    }
}
