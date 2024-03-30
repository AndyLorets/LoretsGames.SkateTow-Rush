using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [Space(5)]
    [SerializeField] private bool _followX, _followY, _followZ;
    [Space(5)]
    [SerializeField] private Vector3 _offset;
    [Space(5)]
    [SerializeField] private UpdateType _updateType; 

    private enum UpdateType { Fixed, Update}

    private void Update()
    {
        if (_updateType != UpdateType.Update)
            return;

        Following();
    }
    void FixedUpdate()
    {
        if (_updateType != UpdateType.Fixed) 
            return;

        Following(); 
    }
    private void Following()
    {
        float x = _followX ? _target.position.x : transform.position.x;
        float y = _followY ? _target.position.y : transform.position.y;
        float z = _followZ ? _target.position.z : transform.position.z;

        Vector3 pos = new Vector3(x, y, z) + _offset; 
        transform.position = pos; 
    }
}
