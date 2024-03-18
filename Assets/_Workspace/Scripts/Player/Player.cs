using System;
using UnityEngine;

public class Player : MonoBehaviour, ITakeDamage
{
    [Header("Parameters")]
    [SerializeField, Range(5, 25)] private int _jumpPower = 10;
    [SerializeField, Range(1, 10)] private int _boostPower = 10;
    [SerializeField] private LayerMask _groundLayer; 

    private Rigidbody _rb;
    private PlayerAnimationsController _animations;

    [Header("Components"), Space(10)]
    [SerializeField] private Rigidbody[] _ragdollRb;
    [SerializeField] private Collider[] _ragdollCol;


    private const float damage_impact_speed = 40f; 
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
    }
    private void OnDisable()
    {
        GameManager.onFinish -= SetFinishParameters;
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
        _animations = new PlayerAnimationsController(this, transform.GetChild(0).GetComponent<Animator>());
        
        SetRagdollState(false);
    }
    private void Update()
    {
        if (!GameManager.isGameStart) return;

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
        if (transform.position.y < 1) 
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }
    private void Jump(Collision other)
    {
        Jumper jumper = other.gameObject.GetComponent<Jumper>();
        if (jumper != null)
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
            jumper.ActiveJumper();

            if (SpeedScore > 0)
                TimerManager.AddTime();

            onTrick?.Invoke();
            CameraManager.ChangeCam(CameraManager.cam_fly_name);
            AudioManager.PlayOneShot(AudioManager.SoundType.Boost);
        }
    }
    private void Boost()
    {
        _rb.AddForce(Vector3.forward * _boostPower, ForceMode.VelocityChange);
        AudioManager.PlayOneShot(AudioManager.SoundType.Boost);
    }
    public void SetRigibodyDrag(float value)
    {
        _rb.drag += value;
    }

    #region GameLogic
    private void SetFinishParameters()
    {
        _rb.drag = 2;
    }
    public void TakeDamage(float damage)
    {
        AudioManager.PlayOneShot(AudioManager.SoundType.Damage);
        if (damage > damage_impact_speed)
            Die();
    }
    private void Die()
    {
        _rb.AddTorque(Vector3.one * 50, ForceMode.Impulse);

        onDie?.Invoke();    
        SetRagdollState(true);

        ServiceLocator.GetService<PlayerHookController>().ClearHookObj();
        GameManager.Lose();
    }
    private void SetRagdollState(bool state)
    {
        for (int i = 0; i < _ragdollRb.Length; i++)
            _ragdollRb[i].isKinematic = !state;

        for (int i = 0; i < _ragdollCol.Length; i++)
            _ragdollCol[i].enabled = state;

        onChangeRagdollState?.Invoke(state); 
    }

    #endregion

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag(ObjTags.collect_tag))
        //    Destroy(other.gameObject);

        //if (other.CompareTag(ObjTags.trick_tag))
        //    onTrick?.Invoke(); 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(ObjTags.jumper_tag))
            Jump(other);

        if (other.gameObject.CompareTag(ObjTags.damage_tag))
            TakeDamage(other.relativeVelocity.magnitude);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(ObjTags.boost_tag))
            Boost();
    }
    #endregion
}
