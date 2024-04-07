using DG.Tweening;
using UnityEngine;

public class GemPrefab : MonoBehaviour
{
    private Player _player;
    private Collider _collider;

    private Vector3 _startLocPos;
    private Transform _parent;

    private bool _onMove; 

    private const float move_duration = .25f;
    private void Start()
    {
        Construct();
        ContanierManager.onReset += ResetAll;
    }
    private void OnDestroy()
    {
        ContanierManager.onReset -= ResetAll;
    }
    private void Construct()
    {
        _player = ServiceLocator.GetService<Player>();
        _collider = GetComponent<Collider>();

        _parent = transform.parent;
        _startLocPos = transform.localPosition;
    }
    private void MoveToPlayer()
    {
        if (_onMove) return;

        _onMove = true;

        transform.parent = _player.transform;
        transform.DOLocalMove(Vector3.zero, move_duration)
            .SetUpdate(UpdateType.Fixed)
            .OnComplete(() => Picked());
    }
    private void Picked()
    {
        gameObject.SetActive(false);
    }
    private void ResetAll()
    {
        transform.localPosition = _startLocPos;
        transform.parent = _parent;
        gameObject.SetActive(true);
        _collider.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ObjTags.player_tag)) return;

        _collider.enabled = false;
        GemManager.AddGem(1, transform.position);
        MoveToPlayer();
    }
}
