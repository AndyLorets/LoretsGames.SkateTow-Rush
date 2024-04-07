using System;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    [SerializeField, Min(5)] private float _cooldownTime = 20f;
    [SerializeField] private string _currentTime;

    private float _cooldownEndTime; 
    public static Action onCoolDown;

    private static CooldownManager _instance;
    public static bool callDownRaedy { get; private set; }
    public static bool callDownInProgress { get; set; } = true; 
    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        _cooldownEndTime = _cooldownTime; 
    }
    private void Update()
    {
        _currentTime = Time.time.ToString();

        if (!callDownInProgress) return; 

        if (Time.time >= _cooldownEndTime)
            CallCoolDown();  
    }

    private void CallCoolDown()
    {
        if (callDownRaedy) return;

        callDownRaedy = true;
        _cooldownEndTime = Time.time + _cooldownTime;
        onCoolDown?.Invoke();
    }

    public static void ResetCallDown()
    {
        _instance._cooldownEndTime = Time.time + _instance._cooldownTime;
        callDownRaedy = false;
    }
}
