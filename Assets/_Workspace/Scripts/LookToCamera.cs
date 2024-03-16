using UnityEngine;

public class LookToCamera : MonoBehaviour
{
    Camera _camera;
    void Awake() => _camera = Camera.main;

    void Update()
    {
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
                    _camera.transform.rotation * Vector3.up);
    }
}
