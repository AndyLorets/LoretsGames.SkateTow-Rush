using DG.Tweening;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BombPrefab : MonoBehaviour, IDroppable
{
    [SerializeField] private ParticleSystem _pickedEffect;

    private Player _player;
    private Collider _collider;

    private bool _onMove; 

    private const float move_duration = .25f;
    private const float deactive_duration = 2f;

    private void Awake()
    {
        _pickedEffect.gameObject.SetActive(false);
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }

    public void Construct(bool active = true)
    {
        _player = ServiceLocator.GetService<Player>();  
        _collider = GetComponent<Collider>();   

        _collider.enabled = false;
        gameObject.SetActive(active);
    }

    public void MoveToPlayer()
    {
        if (_onMove) return;

        _onMove = true;
        _collider.enabled = true;
        gameObject.SetActive(true);
        transform.parent = _player.transform;
        transform.DOLocalMove(Vector3.zero, move_duration)
            .SetUpdate(UpdateType.Fixed)
            .OnComplete(() => Picked());
    }

    private void Picked()
    {
        _pickedEffect.transform.parent = null;
        _pickedEffect.gameObject.SetActive(true); 
        _pickedEffect.Play(); 
        _player.ClearHookObj();
        transform.parent = null;
        transform.DOJump(_player.transform.position + Vector3.right + Vector3.up * 5, 1, 1, deactive_duration)
            .OnComplete(() => gameObject.SetActive(false)); 
    }
    //private void CheckDamageRadius()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, damage_radius);

    //    foreach (Collider collider in colliders)
    //    {
    //        ITakeDamage damageReceiver = collider.GetComponent<ITakeDamage>();
    //        damageReceiver?.TakeDamage();
    //    }
    //}
}
