using UnityEngine;
using DG.Tweening; 

public class CoinPrefab : MonoBehaviour
{
    private Player _player;
    private Collider _collider;

    private Vector3 _startLocPos;
    private Transform _parent; 

    private bool _onMove;
    private bool _isInit; 

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
    public void Construct()
    {
        if (_isInit) return; 

        _player = ServiceLocator.GetService<Player>();
        _collider = GetComponent<Collider>();

        _isInit = true;

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
        _onMove = false; 
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
        MoneyManager.Add(1, transform.position);
        MoveToPlayer();
    }
}
