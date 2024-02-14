using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBase : MonoBehaviour
{
    private Vector3 _startLocalPos; 

    protected virtual void Start()
    {
        Construct();
    }
    protected virtual void Construct()
    {
        _startLocalPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0); 
    }
    protected virtual void Restart()
    {
        transform.localPosition = _startLocalPos;
    }
}
