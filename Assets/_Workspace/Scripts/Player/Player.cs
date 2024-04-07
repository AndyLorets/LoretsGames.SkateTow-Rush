using DG.Tweening;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour, ITakeDamage
{
    [Header("Parameters")]
    [SerializeField, Range(5, 25)] private int _jumpPower = 10;
    [SerializeField, Range(80, 120)] private int _boostSpeed = 100;
    [SerializeField] private LayerMask _groundLayer; 
    [Space(10)]
    [SerializeField, Range(5, 15)] private float _clampVelocityX = 10;
    [SerializeField, Range(50, 60)] private float _clampVelocityY = 55;
    [SerializeField, Range(50, 70)] private float _clampVelocityZ = 60;

    private Rigidbody _rb;
    private Collider _col; 
    private PlayerAnimationsController _animations;

    [Header("RagDoll"), Space(10)]
    [SerializeField] private Rigidbody[] _ragdollRb;
    [SerializeField] private Collider[] _ragdollCol;

    [Header("Effects"), Space(10)]
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private ParticleSystem _respawnEffect;

    private bool _canTakeDamage = false;
    private bool _isJump = true;
    public bool isBoost { get; private set; }

    private const float damage_impact_speed = 40f;
    private const float boost_delay = 1.5f;
    private const float alive_delay = 3f;
    private const float min_yPos = 1.15f;
    private float _velocitySpeed => _rb.velocity.magnitude;
    public int SpeedScore => Mathf.RoundToInt(_velocitySpeed);
    private bool RayIsGround => Physics.Raycast(transform.position, Vector3.down, 3f, _groundLayer);
    public bool onGround { get; private set; } = true;

    public Action<bool> onChangeRagdollState;
    public Action onTrick;
    public Action onDie;
    public Action OnFly; 
    public Action OnLanding;
    public Action onDestroy;

    private Tweener _boostTween;
    private Tweener _returtBoostTween;

    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {
        Construct();
    }
    private void OnEnable()
    {
        GameManager.onFinish += SetFinishParameters;
        GameManager.onGameStart += BoostImmortal; 
    }
    private void OnDisable()
    {
        GameManager.onFinish -= SetFinishParameters;
        GameManager.onGameStart -= BoostImmortal;
    }

    private void OnDestroy()
    {
        onDestroy.Invoke();
    }
    private void Initialize()
    {
        ServiceLocator.RegisterService(this);
    }
    private void Construct()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();    
        _animations = new PlayerAnimationsController(this, transform.GetChild(0).GetComponent<Animator>());

        SetRagdollState(false);
    }
    private void Update()
    {
        if (!GameManager.isGameStart) 
            return;

        if (RayIsGround && !onGround)
        {
            onGround = true;
            CameraManager.ChangeCam(CameraManager.cam_game_name);
            OnLanding?.Invoke();
        }
        if (!RayIsGround && onGround)
        {
            onGround = false;
            OnFly?.Invoke(); 
        }

        bool isMoving = _rb.velocity.magnitude > .5f;
        _animations.Moving(isMoving);
    }
    void FixedUpdate()
    {
        if (transform.position.y < min_yPos)
            transform.position = new Vector3(transform.position.x, min_yPos, transform.position.z);
        else if (!_isJump && transform.position.y > min_yPos)
            transform.position = new Vector3(transform.position.x, min_yPos, transform.position.z);

        if (!GameManager.isGameStart)
            return;

        ClampVelocity(); 
    }
    private void BoostImmortal()
    {
        _canTakeDamage = false;
        Boost();
        Invoke(nameof(Alive), alive_delay);
    }
    private void Alive()
    {
        _canTakeDamage = true;
    }
    private void Jump(Collision other)
    {
        _isJump = true; 

        Jumper jumper = other.gameObject.GetComponent<Jumper>();
        if (jumper != null)
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
            jumper.ActiveJumper();

            onTrick?.Invoke();
            CameraManager.ChangeCam(CameraManager.cam_fly_name);
            AudioManager.PlayOneShot(AudioManager.SoundType.Boost);
        }
    }
    private void Boost()
    {
        isBoost = false;
        StartCoroutine(nameof(BoostActivity)); 
        AudioManager.PlayOneShot(AudioManager.SoundType.Boost);
    }
    private IEnumerator BoostActivity()
    {
        isBoost = true;
        float currentVelZ = _rb.velocity.z;
 
        _boostTween?.Kill();
        _returtBoostTween?.Kill();
        _boostTween = DOTween.To(() => currentVelZ, x => currentVelZ = x, _boostSpeed, boost_delay)
            .OnComplete(delegate ()
            {
                _returtBoostTween = DOTween.To(() => currentVelZ, x => currentVelZ = x, _clampVelocityZ, boost_delay)
                .OnComplete(() => isBoost = false);
            }); 

        while (isBoost)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, currentVelZ);
            yield return null; 
        }
    }
    private void ClampVelocity()
    {
        if (isBoost) return;

        float clampX = _rb.velocity.x;
        clampX = Mathf.Clamp(clampX, -_clampVelocityX, _clampVelocityX);
        float clampY = _rb.velocity.y;
        clampY = Mathf.Clamp(clampY, -_clampVelocityY, _clampVelocityY);
        float clampZ = _rb.velocity.z;
        clampZ = Mathf.Clamp(clampZ, -4, _clampVelocityZ);
        _rb.velocity = new Vector3(clampX, clampY, clampZ);
    }
    private void SetFinishParameters()
    {
        _rb.drag = 2;
        _boostTween?.Kill();
        isBoost = false;
        StopCoroutine(nameof(BoostActivity));
    }
    public void TakeDamage(float damage)
    {
        if (!_canTakeDamage) return;

        _hitEffect?.Play(); 
        AudioManager.PlayOneShot(AudioManager.SoundType.Damage);

        if (damage > damage_impact_speed)
            Die();
    }
    private void Die()
    {
        _canTakeDamage = false; 
        _rb.drag = 5;
        isBoost = false; 
        onDie?.Invoke();    
        SetRagdollState(true);

        ServiceLocator.GetService<PlayerHookController>().ClearHookObj();
        GameManager.Lose();
    }
    public void Respawn()
    {
        _respawnEffect?.Play(); 
        _rb.drag = 0;
        SetRagdollState(false);
        BoostImmortal(); 
    }
    public void RespawnAfterEnd()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, 5);
        Vector3 velocity = _rb.velocity;

        _respawnEffect?.Play();
        transform.position = pos;
        _rb.velocity = velocity; 

        BoostImmortal();
    }
    private void SetRagdollState(bool state)
    {
        for (int i = 0; i < _ragdollRb.Length; i++)
            _ragdollRb[i].isKinematic = !state;

        for (int i = 0; i < _ragdollCol.Length; i++)
            _ragdollCol[i].enabled = state;

        onChangeRagdollState?.Invoke(state); 
    }

    #region Collisions

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(ObjTags.jumper_tag))
            Jump(other);

        if (other.gameObject.CompareTag(ObjTags.damage_tag))
            TakeDamage(other.relativeVelocity.magnitude);

        if (other.gameObject.CompareTag(ObjTags.ground_tag) && _isJump)
            _isJump = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ObjTags.boost_tag))
            Boost();
    }
    #endregion
}
