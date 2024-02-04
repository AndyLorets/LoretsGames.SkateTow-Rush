using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour, ITakeDamage
{
    [Header("Parameters")]
    [SerializeField, Range(40, 70)] private int _maxSpeed = 50;
    [SerializeField, Range(20, 40)] private int _rotateSpeed = 30;
    [SerializeField, Range(5, 25)] private int _jumpPower = 10;
    [SerializeField, Range(1, 10)] private int _boostPower = 10;
    [SerializeField] private LayerMask _groundLayer; 

    private Transform _hookObj;
    private Rigidbody _rb;
    private PlayerAnimationsController _animations;

    [Header("Components"), Space(10)]
    [SerializeField] private Rigidbody[] _ragdollRb;
    [SerializeField] private Collider[] _ragdollCol;
    //[SerializeField] private TrailRenderer _trailRenderer;

    private const float damage_impact_speed = 40f; 
    private const float min_distance_toHook = 5f;
    private const float clamp_velocity_x = 10;

    private float CheckDistanceToHook => Vector3.Distance(transform.position, _hookObj.position);
    private bool IsGround => Physics.Raycast(transform.position, Vector3.down, 3f, _groundLayer);
    private float _velocitySpeed => _rb.velocity.magnitude;
    public int SpeedScore => Mathf.RoundToInt(_velocitySpeed);

    private bool _onGround = true;
    private bool _isStarting;

    public Action<bool> onChangeRagdollState;
    public Action onTrick;
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
        GameManager.onFinish += Finish; 
    }
    private void OnDisable()
    {
        GameManager.onFinish -= Finish;
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
        if (!_isStarting) return;

        if (IsGround && !_onGround)
        {
            _onGround = true;
            CameraManager.ChangeCam(CameraManager.cam_game_name);
            OnLanding?.Invoke();
        }
        if (!IsGround && _onGround)
            _onGround = false;

        bool isMoving = _rb.velocity.magnitude > .5f;
        _animations.Moving(isMoving);
    }
    void FixedUpdate()
    {
        if (transform.position.y < 1) 
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);

        if (_isStarting)
            Move();
    }

    #region Hook and Move
    private void Move()
    {
        if (_hookObj == null) 
        { 
            if (_isStarting) 
                LookAtForce(Vector3.forward);
            return; 
        }

        Vector3 force = _hookObj.position - transform.position;

        AddForce(force);
        LookAtForce(force);
        ClampVelocity();

        if (CheckDistanceToHook < min_distance_toHook || _hookObj.transform.position.z <= transform.position.z)
            ClearHookObj();
    }
    private void AddForce(Vector3 force)    
    {
        force.y = 0;
        force.z = Mathf.Clamp(force.z, 0, _maxSpeed / 2); 

        if (_rb.velocity.magnitude < _maxSpeed)
            _rb.AddForce(force, ForceMode.Acceleration);

        force.z = 0;
        _rb.AddForce(force * (_rb.velocity.magnitude * .05f), ForceMode.Acceleration);
    }
    private void LookAtForce(Vector3 force)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(force), Time.deltaTime * _rotateSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    private void ClampVelocity()
    {
        float clampX = _rb.velocity.x;
        clampX = Mathf.Clamp(clampX, -clamp_velocity_x, clamp_velocity_x);
        _rb.velocity = new Vector3(clampX, _rb.velocity.y, _rb.velocity.z);
    }
    public void SetRigibodyDrag(float value)
    {
        _rb.drag += value; 
    }
    public void SetHookObje(Transform hook)
    {
        _hookObj = hook;
        Vector3 force = _rb.velocity;
        force.x = force.x * .7f; 
        _rb.velocity = force;
        GrapplingBehaviour.StartGrapple(_hookObj);

        if (!_isStarting)
        {
            _isStarting = true;
            CameraManager.ChangeCam(CameraManager.cam_game_name);
        }
    }
    public void ClearHookObj()
    {
        if(_hookObj == null) return;    

        _hookObj = null;
        GrapplingBehaviour.StopGrapple(); 
    }
    private void Jump(Collision other)
    {
        Jumper jumper = other.gameObject.GetComponent<Jumper>();
        if (jumper != null)
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
            jumper.ActiveJumper();

            if(SpeedScore > 0)
                ScoreManager.AddScore(SpeedScore, transform.position);

            onTrick?.Invoke();
            CameraManager.ChangeCam(CameraManager.cam_fly_name);
        }
    }
    private void Boost()
    {
        _rb.AddForce(Vector3.forward * _boostPower, ForceMode.VelocityChange);
    }

    #endregion

    #region GameLogic
    private void Finish()
    {
        _rb.drag = 2;

        _isStarting = false;
    }
    public void TakeDamage(float damage)
    {
        if (damage > damage_impact_speed)
            Lose();
    }
    private void Lose()
    {
        _rb.AddTorque(Vector3.one * 50, ForceMode.Impulse);

        ClearHookObj(); 
        SetRagdollState(true);

        _isStarting = false;

        ScoreManager.AddScore(-15, transform.position + Vector3.forward * 20);
        ServiceLocator.GetService<GameManager>().Lose();
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
