using System;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    [SerializeField, Range(10, 30)] private int _lookAtSpeed = 20;
    [Space(5)]
    [SerializeField, Range(0.04f, 0.1f)] private float _horizontalSpeedCoefficient = 0.08f;

    private Vector3 _hookOffset; 
    private Transform _hookObj;
    private Rigidbody _rb;
    private Vector3 HookPosition => _hookObj.position + _hookOffset;
    private Vector3 HookDirection => HookPosition - transform.position;

    private const float x_hook_dist = 15f;
    private const float y_max_hook_dist = 20f;
    private const float y_min_hook_dist = 0f;
    private const float z_max_hook_dist = 120f;
    private const float z_min_hook_dist = 6f;
    private const float x_force_clamp = 6f;

    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        _rb = GetComponent<Rigidbody>();    
    }
    private void FixedUpdate()
    {
        if (!GameManager.isGameStart)
            return;

        Move();
        HookDistCheker(); 
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.isGameStart)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point = hit.point;
                point.x = Mathf.Clamp(point.x, -x_hook_dist, x_hook_dist);
                point.z = Mathf.Clamp(point.z, transform.position.z + (z_min_hook_dist * z_min_hook_dist), transform.position.z + z_max_hook_dist);
                SetHook(hit.transform, point); 
            }
        }
        if (Input.GetMouseButtonUp(0))
            ClearHookObj(); 
    }
    private void Move()
    {
        if (_hookObj == null)
        {
            LookAtForce(Vector3.forward);
            return;
        }

        AddForce(HookDirection);
        LookAtForce(HookDirection);
    }
    private void AddForce(Vector3 forceDirection)
    {
        forceDirection.y = 0;
        _rb.AddForce(forceDirection, ForceMode.Acceleration);
        forceDirection.z = 0;
        forceDirection.x = Mathf.Clamp(forceDirection.x, -x_force_clamp, x_force_clamp);
        _rb.AddForce(forceDirection * (_rb.velocity.magnitude * _horizontalSpeedCoefficient), ForceMode.Acceleration);
    }

    private void LookAtForce(Vector3 force)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(force), Time.deltaTime * _lookAtSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    private void SetHook(Transform hook, Vector3 offset)
    {
        Vector3 hookOffset = offset - hook.transform.position;

        _hookOffset = hookOffset;
        _hookObj = hook;

        GrapplingBehaviour.StartGrapple(_hookObj, _hookOffset);
    }
    private void HookDistCheker()
    {
        if(_hookObj == null) return;

        bool cutHook = HookPosition.z <= transform.position.z + z_min_hook_dist; 
        if (cutHook)
            ClearHookObj();
    }
    public void ClearHookObj()
    {
        if (_hookObj == null) return;

        _hookObj = null;
        GrapplingBehaviour.StopGrapple();
    }
}
