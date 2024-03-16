using System;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    [SerializeField, Range(20, 40)] private int _rotateSpeed = 30;

    private float _moveMaxSpeed; 

    private Transform _hookObj;
    private Rigidbody _rb;

    private const float min_distance_toHook = 5f;
    private float CheckDistanceToHook => Vector3.Distance(transform.position, _hookObj.position);

    private const ItemType _itemType = ItemType.UpgradeMoveSpeed; 

    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        _rb = GetComponent<Rigidbody>();    
    }
    private void Start()
    {
        ShopManager.OnUpgrade += SetMaxSpeed; 

        Construct(); 
    }
    private void OnDestroy()
    {
        ShopManager.OnUpgrade -= SetMaxSpeed;
    }
    private void Construct()
    {
        _moveMaxSpeed = GameDataManager.UpgradeValue.GetValue(ItemConvertor.ConvertTitleFromType(_itemType));
    }
    private void FixedUpdate()
    {
        if (GameManager.isGameStart)
            Move();
    }
    private void Move()
    {
        if (_hookObj == null)
        {
            if (GameManager.isGameStart)
                LookAtForce(Vector3.forward);
            return;
        }

        Vector3 hookDirection = _hookObj.position - transform.position;
        AddForce(hookDirection);
        LookAtForce(hookDirection);

        if (CheckDistanceToHook < min_distance_toHook || _hookObj.transform.position.z <= transform.position.z)
            ClearHookObj();
    }
    private void AddForce(Vector3 forceDirection)
    {
        forceDirection.y = 0;
        forceDirection.z = Mathf.Clamp(forceDirection.z, 0, _moveMaxSpeed / 2);

        if (_rb.velocity.magnitude < _moveMaxSpeed)
            _rb.AddForce(forceDirection, ForceMode.Acceleration);

        forceDirection.z = 0;
        _rb.AddForce(forceDirection * (_rb.velocity.magnitude * .035f), ForceMode.Acceleration);
    }
    private void LookAtForce(Vector3 force)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(force), Time.deltaTime * _rotateSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    public void SetHookObje(Transform hook)
    {
        if (!GameManager.isGameStart && !GameManager.isGameOver)
            GameManager.StartGame();

        _hookObj = hook;
        Vector3 force = _rb.velocity;
        force.x = force.x * .7f;
        _rb.velocity = force;
        GrapplingBehaviour.StartGrapple(_hookObj);
    }
    public void ClearHookObj()
    {
        if (_hookObj == null) return;

        _hookObj = null;
        GrapplingBehaviour.StopGrapple();
    }

    private void SetMaxSpeed(ItemType itemType, int value, int maxValue)
    {
        if (itemType != _itemType) return;

        string key = ItemConvertor.ConvertTitleFromType(_itemType);
        int lastValue = GameDataManager.UpgradeValue.GetValue(key);
        int endValue = Math.Clamp(lastValue + value, lastValue + value, maxValue);
        GameDataManager.UpgradeValue.SetValue(key, endValue);
        _moveMaxSpeed = GameDataManager.UpgradeValue.GetValue(key); 
    }
}
