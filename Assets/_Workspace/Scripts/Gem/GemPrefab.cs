using DG.Tweening;
using UnityEngine;

public class GemPrefab : MonoBehaviour
{
    [SerializeField] private ParticleSystem _pickedEffect;

    private Player _player;
    private Collider _collider; 

    private bool _onMove; 

    private const float move_duration = .25f;

    private void Start()
    {
        Construct(); 
    }

    private void Construct()
    {
        _player = ServiceLocator.GetService<Player>();
        _collider = GetComponent<Collider>();
    }
    private void MoveToPlayer()
    {
        if (_onMove) return;

        _onMove = true;
        gameObject.SetActive(true);
        transform.parent = _player.transform;
        transform.DOLocalMove(Vector3.zero, move_duration)
            .SetUpdate(UpdateType.Fixed)
            .OnComplete(() => Picked());
    }
    private void Picked()
    {
        gameObject.SetActive(false);

        if (_pickedEffect == null) return; 
        _pickedEffect.transform.parent = null;
        _pickedEffect.gameObject.SetActive(true); 
        _pickedEffect.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ObjTags.player_tag)) return;

        _collider.enabled = false;
        GemManager.AddGem(1, transform.position);
        MoveToPlayer();
    }
}
