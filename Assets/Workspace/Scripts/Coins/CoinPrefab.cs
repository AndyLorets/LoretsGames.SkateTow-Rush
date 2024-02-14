using UnityEngine;
using DG.Tweening; 

public class CoinPrefab : MonoBehaviour
{
    [SerializeField] private ParticleSystem _pickedEffect; 

    private Player _player;
    private Collider _collider;

    private bool _onMove;
    private bool _isInit; 

    private const float move_duration = .25f;

    private void Start() => Init();
    public void Init()
    {
        if (_isInit) return; 

        _player = ServiceLocator.GetService<Player>();
        _collider = GetComponent<Collider>();

        _isInit = true;
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
        _pickedEffect.transform.parent = null; 
        _pickedEffect.gameObject.SetActive(true);
        _pickedEffect.Play(); 
        _onMove = false; 
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ObjTags.player_tag)) return;

        _collider.enabled = false;
        CoinsManager.AddCoins(1, transform.position);
        MoveToPlayer();
    }
}
