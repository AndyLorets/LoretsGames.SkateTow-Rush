using System;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    [SerializeField, Range(20, 40)] private int _rotateSpeed = 30;

    private float _moveMaxSpeed;

    private Vector3 _hookOffset; 
    private Transform _hookObj;
    private Rigidbody _rb;

    private const ItemType _itemType = ItemType.UpgradeMoveSpeed;
    private Vector3 HookDirection => (_hookObj.position + _hookOffset) - transform.position;

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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                SetHookObje(hit.transform, hit.point); 
                //Debug.Log("Name of clicked object: " + hit.transform.name);
            }
        }
        if (Input.GetMouseButtonUp(0))
            ClearHookObj(); 
    }
    private void Move()
    {
        if (_hookObj == null)
        {
            if (GameManager.isGameStart)
                LookAtForce(Vector3.forward);
            return;
        }

        AddForce(HookDirection);
        LookAtForce(HookDirection);
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
    private void SetHookObje(Transform hook, Vector3 offset)
    {
        Vector3 hookOffset = offset - hook.transform.position;

        _hookOffset = hookOffset;
        _hookObj = hook;

        Vector3 force = _rb.velocity;
        force.x = force.x * .7f;
        _rb.velocity = force;

        GrapplingBehaviour.StartGrapple(_hookObj, _hookOffset);
        if (!GameManager.isGameStart && !GameManager.isGameOver)
            GameManager.StartGame();
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
